using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MALShowIDCrawler.Properties;

namespace MALShowIDCrawler
{
	[Serializable]
	class Crawler
	{
		private Dictionary<int, string> _animeInfo;
		private int _id;

		public Crawler(Crawler crawler)
		{
			_animeInfo = crawler.AnimeInfo;
			_id = crawler._id;
		}

		public Crawler()
		{
			_animeInfo = new Dictionary<int, string>();
			_id = 1;
		}

		/// <summary>
		/// Begins to crawl the database, and raises an event upon completion
		/// </summary>
		/// <param name="maxId"></param>
		/// <param name="maxFailures"></param>
		/// <param name="saveFilePath"></param>
		public void Run(int maxId, int maxFailures, string saveFilePath)
		{
			Task.Run(() =>
			{
				CrawlIDs(maxId, maxFailures);
			})
			.ContinueWith(temp =>
			{
				OnCrawlerFinished(new EventArgs());
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		public Dictionary<int, string> AnimeInfo
		{
			get { return _animeInfo; }
		}

		public event EventHandler CrawlerFinished;
		protected void OnCrawlerFinished(EventArgs e)
		{
			CrawlerFinished?.Invoke(this, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="maxIds"></param>
		/// <param name="maxFailures"></param>
		void CrawlIDs(int maxIds, int maxFailures)
		{
			int fails = 0;
			//if we've previously queried all ids up to the max id, ask user if they want to requery
			//if yes, set id back to 1, remove all the anime entries up to maxId,
			//then requery up to maxId.  this will leave all the rest of the entries with Ids higher untouched
			//if no, don't crawl at all.
			if (maxIds <= _id)
			{
				var result = MessageBox.Show(
					Resources.CrawlerMaxIDTooSmall,
					"MAL Crawler",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question);

				if (result == MessageBoxResult.No)
					return;

				for (int i = 1; i < maxIds; i++)
				{
					if (_animeInfo.ContainsKey(i))
						_animeInfo.Remove(i);
				}
				_id = 1;
			}
			for (int i = _id; i <= maxIds; i++)
			{
				HttpWebRequest request = (HttpWebRequest) WebRequest.Create(string.Format(Resources.AnimeURL, i));
				request.AutomaticDecompression = DecompressionMethods.GZip;
				request.Proxy = null;
				try
				{
					using (var response = (HttpWebResponse) request.GetResponse())
					using (var stream = response.GetResponseStream())
					{
						if (stream != null)
							using (var buffer = new BufferedStream(stream))
							using (var reader = new StreamReader(buffer))
							{
								while (reader.ReadLine() != "<title>")
								{ }
								var html = reader.ReadLine();
								var title = html?.Substring(0, html.IndexOf(" - MyAnime", StringComparison.Ordinal));
								if (_animeInfo.ContainsKey(i))
									_animeInfo[i] = title;
								else
									_animeInfo.Add(i, title);
								Console.WriteLine(Resources.IDStatusTitle, i, response.StatusCode, title);
							}
					}
					fails = 0;
				}
				catch (WebException e)
				{
					Console.WriteLine(Resources.IDStatus, i, e.Status);
					fails++;
				}
				catch (ArgumentException)
				{
					//ignored (for now)
				}

				//we've failed too many times, set the id to the last id before the consecutive failures.
				if (fails >= maxFailures)
				{
					_id = i - maxFailures + 1;
					return;
				}
				Thread.Sleep(20);
			}
			_id = maxIds;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static Crawler LoadData(string filePath)
		{
			try
			{
				if (File.Exists(filePath) == false)
					return new Crawler();

				using (var fs = new FileStream(filePath, FileMode.Open))
				using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
				{
					var entry = archive.GetEntry(Resources.DataBaseFileName);
					if (entry == null)
						return new Crawler();

					var formatter = new BinaryFormatter();
					return formatter.Deserialize(entry.Open()) as Crawler ?? new Crawler();
				}
			}
			catch
			{
				return new Crawler();
			}
		}

		/// <summary>
		/// Loads an old dbinfo file.  Will throw an exeption
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static Crawler LoadLegacyData(string filePath)
		{
			using (var fs = new FileStream(filePath, FileMode.Open))
			{
				var formatter = new BinaryFormatter();
				var c = formatter.Deserialize(fs) as Crawler;
				if (c == null)
					throw new SerializationException("Deserialized object must be of type Crawler.");
				return c;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SaveData(ZipArchive archive)
		{
			try
			{
				var formatter = new BinaryFormatter();
				var entry = archive.GetEntry(Resources.DataBaseFileName) ??
					archive.CreateEntry(Resources.DataBaseFileName);

				formatter.Serialize(entry.Open(), this);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
