using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.IO.Compression;
using System.Net;
using Microsoft.Win32;

namespace MALShowIDCrawler
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	// ReSharper disable once RedundantExtendsListEntry
	public partial class MainWindow : Window
	{
		private bool _rememberInfo;
		private string _dateCrawled = "Unknown";
		private Crawler _dataBase;
		private int _currentId = 1;

		/// <summary>
		/// Initialize UI
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			LoadData();
			Closing += SaveData;

			_dataBase = Crawler.LoadData(Path.Combine(SaveFilePath, Properties.Resources.SettingsFileName));
			SetTitleText();
		}

		private static string SaveFilePath
		{
			get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MALCrawler"); }
		}

		/// <summary>
		/// File accessed date of the dbinfo file
		/// </summary>
		/// <returns></returns>
		private void SetFileDate(StreamReader reader)
		{
			try
			{
				DataFileCreatedText.Text = string.Format(Properties.Resources.FileAccessed,
					reader.EndOfStream ? "Unknown" : reader.ReadLine());
			}
			catch
			{
				DataFileCreatedText.Text = string.Format(Properties.Resources.FileAccessed, "Unknown");
			}
		}

		/// <summary>
		/// Sets the title text to the current indexed position in the database.
		/// </summary>
		private void SetTitleText()
		{
			if (_dataBase.AnimeInfo.Any() == false)
			{
				AnimeTitle.Text = "No database information.";
				return;
			}

			if (_currentId > _dataBase.AnimeInfo.Last().Key)
				_currentId = _dataBase.AnimeInfo.Last().Key;
			else if (_currentId < 1)
				_currentId = 1;

			if (_dataBase.AnimeInfo.ContainsKey(_currentId) == false)
			{
				//the last element's key can be bigger than its index in the dictionary
				while (_dataBase.AnimeInfo.ContainsKey(_currentId) == false && _currentId < _dataBase.AnimeInfo.Last().Key)
					_currentId++;
			}
			try
			{
				AnimeTitle.Text = _dataBase.AnimeInfo[_currentId]
					.Replace("&#039;", "'")
					.Replace("&quot;", "\"")
					.Replace("&amp;", "&");
				IdText.Text = string.Format(Properties.Resources.MalID, _currentId);
			}
			catch
			{
				System.Diagnostics.Debugger.Break();
			}
		}

		/// <summary>
		/// Check to ensure number values in text boxes are all digits
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static bool AreCharsDigits(string str)
		{
			return str.All(char.IsDigit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnCrawlerFinished(object sender, EventArgs e)
		{
			ButtonProgressBar.Visibility = Visibility.Hidden;
			CrawlButton.IsEnabled = true;

			var c = sender as Crawler;
			if (c == null)
			{
				MessageBox.Show("Database crawl failed with an error.",
					"Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			MessageBox.Show("Database crawl finished.",
					"Done", MessageBoxButton.OK, MessageBoxImage.Information);
			
			c.CrawlerFinished -= OnCrawlerFinished;
			_dataBase = c;
			SetTitleText();
			_dateCrawled = DateTime.Now.ToShortDateString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Crawl_OnClick(object sender, RoutedEventArgs e)
		{
			var maxId = MaxIdBox.Text;
			if (string.IsNullOrEmpty(maxId) || AreCharsDigits(maxId) == false)
			{
				MessageBox.Show("Maximum ID should be an integer value.", 
					"Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			var maxFails = StopAfterErrorsBox.Text;
			if (string.IsNullOrEmpty(maxFails) || AreCharsDigits(maxFails) == false)
			{
				MessageBox.Show("Maximum consecutive errors should be an integer value.", 
					"Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			ButtonProgressBar.Visibility = Visibility.Visible;
			var c = new Crawler(_dataBase);
			c.CrawlerFinished += OnCrawlerFinished;
			c.Run(Convert.ToInt32(maxId), Convert.ToInt32(maxFails), SaveFilePath);
			CrawlButton.IsEnabled = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
		{
			_rememberInfo = !_rememberInfo;
		}

		/// <summary>
		/// Increments the currentID and sets the title.
		/// </summary>
		private void AdvanceTitle()
		{
			_currentId++;
			SetTitleText();
		}

		/// <summary>
		/// Jumps to the first listing that satisfied a search term.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="useCurrentId"></param>
		private void Search(string query, bool useCurrentId)
		{
			bool caseSensitive = CaseSensitiveCheckBox.IsChecked ?? false;
			KeyValuePair<int, string> result;
			if (caseSensitive)
			{
				result = useCurrentId
					? _dataBase.AnimeInfo.FirstOrDefault(i => i.Value.Contains(query) && i.Key > _currentId)
					: _dataBase.AnimeInfo.FirstOrDefault(i => i.Value.Contains(query));
			}
			else
			{
				result = useCurrentId
					? _dataBase.AnimeInfo.FirstOrDefault(i => i.Value.ToLower().Contains(query.ToLower()) && i.Key > _currentId)
					: _dataBase.AnimeInfo.FirstOrDefault(i => i.Value.ToLower().Contains(query.ToLower()));
			}

			//if we cant find a value ahead of the current id, try to find one behind it.
			if (result.Value == null)
			{
				if (caseSensitive && useCurrentId)
				{
					result =  _dataBase.AnimeInfo.LastOrDefault(i => i.Value.Contains(query) && i.Key < _currentId);
				}
				else if (useCurrentId)
				{
					result = _dataBase.AnimeInfo.LastOrDefault(i => i.Value.ToLower().Contains(query.ToLower()) && i.Key < _currentId);
				}
			}

			if (result.Value == null)
			{
				//MessageBox.Show(string.Format("Unable to find anything matching \"{0}\".", query),
				//	"Search", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			_currentId = result.Key;
			SetTitleText();
		}

		/// <summary>
		/// Sets the ID to the specified value.
		/// </summary>
		/// <param name="id"></param>
		private void GoToId(string id)
		{
			if (string.IsNullOrEmpty(id))
				return;

			if (id.First().Equals('-') && AreCharsDigits(id.Substring(1)))
			{
				_currentId = 1;
				SetTitleText();
				return;
			}

			if (AreCharsDigits(id) == false)
				return;

			_currentId = Convert.ToInt32(id);
			SetTitleText();
		}

		private bool AddShowToUserList(string username, string password, out string errorMessage)
		{
			errorMessage = "";
			var data = new StringBuilder();
			data.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			data.AppendFormat("<entry>");
			data.AppendLine("<episode>0</episode>");
			data.AppendLine("<status>2</status>");
			data.AppendLine("<score>0</score>");
			data.AppendLine("</entry>");

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}?data={1}", string.Format(Properties.Resources.AddAnimeURL, _currentId), data));
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "GET";
			request.Credentials = new NetworkCredential(username, password);
			request.AutomaticDecompression = DecompressionMethods.GZip;
			request.Proxy = null;

			try
			{
				request.GetResponse();
				return true;
			}
			catch (Exception ex)
			{
				var webException = ex as WebException;
				if (webException == null)
					return false;

				using (var stream = webException.Response.GetResponseStream())
					if (stream != null)
						using (var sr = new StreamReader(stream))
						{
							var response = sr.ReadToEnd();
							errorMessage = string.Format("{0}\n\n{1}", ex.Message, response);
						}
				return false;
			}
		}

		/// <summary>
		/// Saves all user data to an archive
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveData(object sender, EventArgs e)
		{
			try
			{
				var path = SaveFilePath;
				if (Directory.Exists(path) == false)
					Directory.CreateDirectory(path);

				var filePath = Path.Combine(path, Properties.Resources.SettingsFileName);
				if (File.Exists(filePath) == false)
					using (File.Create(filePath)) { }

				using (var fs = new FileStream(filePath, FileMode.Open))
				using (var archive = new ZipArchive(fs, ZipArchiveMode.Update))
				{
					_dataBase.SaveData(archive);
					if (_rememberInfo)
						Encrypter.SavePassword(archive, PasswordBox.Password);

					var entry = archive.GetEntry("settings");
					entry?.Delete();
					entry = archive.CreateEntry("settings");
					using (var writer = new StreamWriter(entry.Open()))
					{
						writer.WriteLine(MaxIdBox.Text);
						writer.WriteLine(StopAfterErrorsBox.Text);
						writer.WriteLine(_currentId);
						writer.WriteLine(RememberInfoCheckBox.IsChecked ?? false ? "1" : "0");
						writer.WriteLine(UserNameBox.Text);
						if (_dateCrawled.Equals("Unknown") == false)
							writer.WriteLine(_dateCrawled);
					}
				}
			}
			catch
			{
				//ignored for now
			}
		}

		/// <summary>
		/// Populate UI with user set values from save file
		/// </summary>
		private void LoadData()
		{
			try
			{
				var path = SaveFilePath;
				if (Directory.Exists(path) == false)
					return;

				var filePath = Path.Combine(path, Properties.Resources.SettingsFileName);
				if (File.Exists(filePath) == false)
				{
					try
					{
						//if we cant find the settings file, try to load up an old database file
						_dataBase = Crawler.LoadLegacyData(Properties.Resources.DataBaseFileName);
						_currentId = 1;
						SetTitleText();
					}
					catch
					{
						//ignored
					}
					return;
				}

				_dataBase = Crawler.LoadData(filePath);

				using (var fs = new FileStream(filePath, FileMode.Open))
				using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
				{
					PasswordBox.Password = Encrypter.GetPassword(archive);

					var entry = archive.GetEntry("settings");
					if (entry == null)
						return;

					using (var reader = new StreamReader(entry.Open()))
					{
						MaxIdBox.Text = reader.ReadLine();
						StopAfterErrorsBox.Text = reader.ReadLine();
						_currentId = Convert.ToInt32(reader.ReadLine());
						var temp = reader.ReadLine();
						_rememberInfo = temp != null && temp.Equals("1");
						RememberInfoCheckBox.IsChecked = _rememberInfo;
						UserNameBox.Text = reader.ReadLine();
						SetFileDate(reader);
					}
				}
			}
			catch
			{
				//ignored
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (_currentId <= 1)
				return;

			_currentId--;
			if (_dataBase.AnimeInfo.ContainsKey(_currentId) == false)
			{
				while (_dataBase.AnimeInfo.ContainsKey(_currentId) == false)
					_currentId--;
			}
			SetTitleText();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NoButton_OnClick(object sender, RoutedEventArgs e)
		{
			AdvanceTitle();
		}

		/// <summary>
		/// Adds the current anime to the user's watched list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void YesButton_Click(object sender, RoutedEventArgs e)
		{
			YesButton.IsEnabled = false;
			ButtonProgressBar.Visibility = Visibility.Visible;
			string response = "";
			bool success = false;
			var user = UserNameBox.Text;
			var password = PasswordBox.Password;
			Task.Run(() =>
			{
				success = AddShowToUserList(user, password, out response);
			})
			.ContinueWith(temp =>
			{
				ButtonProgressBar.Visibility = Visibility.Hidden;
				if (success == false && string.IsNullOrEmpty(response) == false)
				{
					MessageBox.Show(response, "Add Error", MessageBoxButton.OK,
						MessageBoxImage.Error);
				}
				else if (success)
					AdvanceTitle();

				YesButton.IsEnabled = true;
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void ApplyOldFile()
		{
			var dlg = new OpenFileDialog
			{
				Multiselect = false,
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
			};
			var result = dlg.ShowDialog() ?? false;
			if (!result)
				return;

			try
			{
				_dataBase = Crawler.LoadLegacyData(dlg.FileName);
				_currentId = 1;
				SetTitleText();
			}
			catch
			{
				MessageBox.Show("Unable to apply this file.", "Apply Database", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GoButton_Click(object sender, RoutedEventArgs e)
		{
			GoToId(GoToIdBox.Text);
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			Search(Searchbox.Text, false);
		}

		private void GoToIdBox_OnKeyUp(object sender, KeyEventArgs e)
		{
			GoToId(GoToIdBox.Text);
		}

		private void Searchbox_OnKeyUp(object sender, KeyEventArgs e)
		{
			Search(Searchbox.Text, true);
		}

		private void NextSearchButton_Click(object sender, RoutedEventArgs e)
		{
			Search(Searchbox.Text, true);
		}

		private void ApplyOldDataBaseFile_OnClick(object sender, RoutedEventArgs e)
		{
			ApplyOldFile();
		}
	}
}
