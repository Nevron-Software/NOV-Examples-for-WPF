using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.Networking;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NHttpExample : NExampleBase
	{
		#region Constructors

		public NHttpExample()
		{
		}
		static NHttpExample()
		{
			NHttpExampleSchema = NSchema.Create(typeof(NHttpExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.FitMode = ENStackFitMode.Last;
			stack.FillMode = ENStackFillMode.Last;

			stack.Add(CreatePredefinedRequestsWidget());
			stack.Add(CreateCustomRequestWidget());

			m_ResponseContentHolder = new NContentHolder();
			stack.Add(m_ResponseContentHolder);
			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// clear button
			NButton button = new NButton("Clear Requests");
			button.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			button.Click += new Function<NEventArgs>(OnClearRequestsListBoxButtonClick);
			stack.Add(button);

			// create the requests list box in which we add the submitted requests.
			m_RequestsListBox = new NListBox();
			stack.Add(m_RequestsListBox);

			return new NGroupBox("Requests", stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
Demonstrates the HTTP protocol wrapper that comes along with. It allows you to make HTTP requests from a single code base.
</p>
";
		}

		#endregion

		#region Implementation

		#region Implementation - User Interface

		private NWidget CreatePredefinedRequestsWidget()
		{
			NGroupBox groupBox = new NGroupBox("Predefined Requests");

			NStackPanel stack = new NStackPanel();
			groupBox.Content = stack;
			stack.Direction = ENHVDirection.LeftToRight;

			// get Google logo
			NButton getGoogleLogoButton = new NButton("Get Google LOGO");
			getGoogleLogoButton.Click += new Function<NEventArgs>(GetGoogleLogoClick);
			stack.Add(getGoogleLogoButton);

			// get Google thml
			NButton getGoogleHtmlButton = new NButton("Get Google HTML");
			getGoogleHtmlButton.Click += new Function<NEventArgs>(GetGoogleHtmlClick);
			stack.Add(getGoogleHtmlButton);

			// get wikipedia logo
			NButton getWikipediaLogoButton = new NButton("Get Wikipedia LOGO");
			getWikipediaLogoButton.Click += new Function<NEventArgs>(OnGetWikipediaLogoClick);
			stack.Add(getWikipediaLogoButton);

			// get wikipedia home page HTML
			NButton getWikipediaHtmlButton = new NButton("Get Wikipedia HTML");
			getWikipediaHtmlButton.Click += new Function<NEventArgs>(OnGetWikipediaHtmlClick);
			stack.Add(getWikipediaHtmlButton);

			// get wikipedia home page HTML
			NButton getNevronPieChartImage = new NButton("Get Nevron Pie Chart Image");
			getNevronPieChartImage.Click += OnGetNevronPieChartImageClick;
			stack.Add(getNevronPieChartImage);

			return groupBox;
		}

		private NWidget CreateCustomRequestWidget()
		{
			NGroupBox groupBox = new NGroupBox("Request URI");

			NDockPanel dock = new NDockPanel();
			groupBox.Content = dock;

			NLabel label = new NLabel("URI:");
			label.VerticalPlacement = ENVerticalPlacement.Center;
			NDockLayout.SetDockArea(label, ENDockArea.Left);
			dock.Add(label);

			m_URLTextBox = new NTextBox();
			m_URLTextBox.Padding = new NMargins(0, 3, 0, 3);
			NDockLayout.SetDockArea(m_URLTextBox, ENDockArea.Center);
			dock.Add(m_URLTextBox);

			NButton submitButton = new NButton("Submit");
			NDockLayout.SetDockArea(submitButton, ENDockArea.Right);
			submitButton.Click += new Function<NEventArgs>(OnSumbitCustomRequestClick);
			dock.Add(submitButton);

			return groupBox;
		}

		#endregion

		#region Implementation - Event Handlers

		private void GetGoogleLogoClick(NEventArgs args)
		{
			// create a HTTP request for the Google logo and subscribe for Completed event
			string googleLogoURI = "https://www.google.com//images//srpr//logo3w.png";
			NHttpWebRequest request = new NHttpWebRequest(googleLogoURI);
			request.Headers[NHttpHeaderFieldName.Accept] = "image/png";
			
			m_URLTextBox.Text = googleLogoURI;

			// create a list box item for the request, prior to submittion and submit the request
			CreateRequestUIItem(request);
			request.Submit();
		}
		private void GetGoogleHtmlClick(NEventArgs args)
		{
			// create a HTTP request for the Google home page
			string googleHtmlURI = "https://www.google.com";
			NHttpWebRequest request = new NHttpWebRequest(googleHtmlURI);

			m_URLTextBox.Text = googleHtmlURI;

			// create a list box item for the request, prior to submition and submit the request
			CreateRequestUIItem(request);
			request.Submit();
		}
		private void OnGetWikipediaLogoClick(NEventArgs args)
		{
			// create a HTTP request for the Wikipedia logo and subscribe for Completed event
			string wikipediaLogoURI = "https://upload.wikimedia.org//wikipedia//commons//6//63//Wikipedia-logo.png";
			NHttpWebRequest request = new NHttpWebRequest(wikipediaLogoURI);

			m_URLTextBox.Text = wikipediaLogoURI;

			// create a list box item for the request, prior to submittion and submit the request
			CreateRequestUIItem(request);
			request.Submit();
		}
		private void OnGetWikipediaHtmlClick(NEventArgs args)
		{
			// create a HTTP request for the Wikipedia home page and subscribe for Completed event
			string wikipediaHtmlURI = "https://en.wikipedia.org/wiki/Main_Page";
			NHttpWebRequest request = new NHttpWebRequest(wikipediaHtmlURI);

			m_URLTextBox.Text = wikipediaHtmlURI;

			// create a list box item for the request, prior to submittion and submit the request
			CreateRequestUIItem(request);
			request.Submit();
		}
		private void OnGetNevronPieChartImageClick(NEventArgs arg)
		{
			// create a HTTP request for the Wikipedia home page and subscribe for Completed event
			string nevronPieChartImage = "https://www.nevron.com//NIMG.axd?i=Chart//ChartTypes//Pie//3D_pie_cut_edge_ring.png";
			NHttpWebRequest request = new NHttpWebRequest(nevronPieChartImage);

			m_URLTextBox.Text = nevronPieChartImage;

			// create a list box item for the request, prior to submittion and submit the request
			CreateRequestUIItem(request);
			request.Submit();
		}
		private void OnSumbitCustomRequestClick(NEventArgs args)
		{
			try
			{
				// create a HTTP request for the custom URI and subscribe for Completed event
				string uri = m_URLTextBox.Text;
				NWebRequest request;
				if (!NWebRequest.TryCreate(new NUri(uri), out request))
				{
					NMessageBox.Show("The specified URI string is not valid for a URI request. Expected was an HTTP or File uri.", "Invalid URI", ENMessageBoxButtons.OK, ENMessageBoxIcon.Error);
					return;
				}
				
				// create a list box item for the request, prior to submittion and submit the request
				CreateRequestUIItem(request);
				request.Submit();
			}
			catch (Exception ex)
			{
				NMessageBox.Show("Failed to submit custom request.\n\nException was: " + ex.Message, "Failed to submit custom request", ENMessageBoxButtons.OK, ENMessageBoxIcon.Error);
			}
		}
		private void OnClearRequestsListBoxButtonClick(NEventArgs args)
		{
			m_RequestsListBox.Items.Clear();
			m_Request2UIItem.Clear();
		}

		#endregion

		#region Implementation - Requests List

		/// <summary>
		/// Called when a request is about to be submitted. Adds a new entry in the requests list box.
		/// </summary>
		/// <param name="request"></param>
		private void CreateRequestUIItem(NWebRequest request)
		{
			NUriRequestItem item = new NUriRequestItem(request, m_ResponseContentHolder);
			m_RequestsListBox.Items.Add(item.ListBoxItem);
			m_Request2UIItem.Add(request, item);
		}


		#endregion

		#endregion

		#region Fields

		/// <summary>
		/// A content holder for the content of the last completed request.
		/// </summary>
		private NContentHolder m_ResponseContentHolder;
		/// <summary>
		/// A text box in which the user enters the URI for a custom request.
		/// </summary>
		private NTextBox m_URLTextBox;
		/// <summary>
		/// The list in which we add information about the sumbitted requests.
		/// </summary>
		private NListBox m_RequestsListBox;
		/// <summary>
		/// A map for the requests 2 list box items.
		/// </summary>
		private NMap<NWebRequest, NUriRequestItem> m_Request2UIItem = new NMap<NWebRequest, NUriRequestItem>();

		#endregion

		#region Schema

		public static readonly NSchema NHttpExampleSchema;

		#endregion

		#region Nested Types

		public class NUriRequestItem
		{
            #region Constructors

            public NUriRequestItem(NWebRequest request, NContentHolder responseContentHolder)
            {
				Request = request;
				ResponseContentHolder = responseContentHolder;

				NGroupBox groupBox = new NGroupBox(new NLabel("URI: " + request.Uri.ToString()));
				groupBox.Header.MaxWidth = 350;
				groupBox.HorizontalPlacement = ENHorizontalPlacement.Fit;				

				NStackPanel stack = new NStackPanel();
				stack.HorizontalPlacement = ENHorizontalPlacement.Fit;
				groupBox.Content = stack;

				NStackPanel hstack = new NStackPanel();
				hstack.Direction = ENHVDirection.LeftToRight;
				hstack.HorizontalPlacement = ENHorizontalPlacement.Fit;
				hstack.FillMode = ENStackFillMode.None;
				hstack.FitMode = ENStackFitMode.Equal;
				stack.Add(hstack);

				// create progress bar
				ProgressBar = new NProgressBar();

				ProgressBar.PreferredHeight = 20;
				ProgressBar.Minimum = 0;
				ProgressBar.Maximum = 100;
				stack.Add(ProgressBar);

				// create status lable
				StatusLabel = new NLabel();
				StatusLabel.Text = " Status: Submitted";
				stack.Add(StatusLabel);

				// create the abort button.
				AbortButton = new NButton("Abort");
				AbortButton.Click += new Function<NEventArgs>(OnAbortRequestButtonClick);
				hstack.Add(AbortButton);

				// create view response headers button
				if (Request is NHttpWebRequest)
				{
					ViewHeadersButton = new NButton("View Response Headers");
					ViewHeadersButton.Click += new Function<NEventArgs>(OnViewResponseHeadersButtonClick);
					hstack.Add(ViewHeadersButton);
				}

				// add item
				NListBoxItem item = new NListBoxItem(groupBox);
				item.BorderThickness = new NMargins(2);
				item.Border = null;
				ListBoxItem = item;

				// hook request events
				request.Completed += new Function<NWebRequestCompletedEventArgs>(OnRequestCompleted);
                request.StartDownload += OnRequestStartDownload;
                request.DownloadProgress += OnRequestDownloadProgress;
                request.EndDownload += OnRequestEndDownload;
			}

            #endregion

            #region Event Handlers - Request
			 
            private void OnRequestEndDownload(NWebRequestDataEventArgs arg)
            {
				ProgressBar.Value = 100;
            }

            private void OnRequestDownloadProgress(NWebRequestDataProgressEventArgs arg)
            {
				double factor = (double)arg.ProgressLength / (double)arg.DataLength;
				ProgressBar.Value = factor * 100.0d;
			}

            private void OnRequestStartDownload(NWebRequestDataEventArgs arg)
            {
				ProgressBar.Value = 0;
				StatusLabel.Text = " Status: Downloading Data";
			}

            #endregion

            #region Event Handlers - Buttons

            /// <summary>
            /// 
            /// </summary>
            /// <param name="args"></param>
            private void OnAbortRequestButtonClick(NEventArgs args)
			{
				Request.Abort();
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="args"></param>
			private void OnViewResponseHeadersButtonClick(NEventArgs args)
			{
				NHttpResponse httpResponse = Response as NHttpResponse;
				if (httpResponse == null)
					return;

				// create a top level window, setup as a dialog
				NTopLevelWindow window = NApplication.CreateTopLevelWindow();
				window.SetupDialogWindow(Request.Uri.ToString(), true);

				// create a list box for the headers
				NListBox listBox = new NListBox();
				window.Content = listBox;

				// fill with header fields
				INIterator<NHttpHeaderField> it = httpResponse.HeaderFields.GetIterator();
				while (it.MoveNext())
				{
					listBox.Items.Add(new NListBoxItem(it.Current.ToString()));
				}

				// open the window
				window.Open();
			}
			/// <summary>
			/// Called by a NHttpRequest when it has been completed.
			/// </summary>
			/// <param name="args"></param>
			private void OnRequestCompleted(NWebRequestCompletedEventArgs args)
			{
				Response = (NHttpResponse)args.Response;

				// highlight the completed item in red
				ListBoxItem.Border = NBorder.CreateFilledBorder(NColor.LightCoral);

				// update the status
				StatusLabel.Text += " Status: " + Response.Status.ToString() + ", Received In: " + (Response.ReceivedAt - Request.SentAt).TotalSeconds.ToString() + " seconds";

				// Disable the Abort button
				AbortButton.Enabled = false;

				// Enable the Headers Button
				ViewHeadersButton.Enabled = true;

				// update the response content holder
				switch (args.Response.Status)
				{
					case ENAsyncResponseStatus.Aborted:
						// request has been aborted by the user -> do nothing.
						break;

					case ENAsyncResponseStatus.Failed:
						// request has failed -> fill content with an error message
						ResponseContentHolder.Content = new NLabel("Request for URI: " + args.Request.Uri.ToString() + " failed. Error was: " + args.Response.ErrorException.ToString());
						break;

					case ENAsyncResponseStatus.Succeeded:
						// request succeded -> fill content with the response content
						NHttpResponse httpResponse = Response as NHttpResponse;
						if (httpResponse != null)
						{
							HandleHttpResponse(httpResponse);
						}
						else
						{
							NFileWebResponse fileResponse = Response as NFileWebResponse;
							if (fileResponse != null)
							{
								HandleFileResponse(fileResponse);
							}
						}
						break;
				}
			}

			#endregion

			#region Implementation - Responses

			/// <summary>
			/// Handles an HTTP response
			/// </summary>
			/// <param name="response"></param>
			private void HandleHttpResponse(NHttpResponse httpResponse)
			{
				try
				{
					// get the Content-Type Http Header field, and split it to portions
					// NOTE: the Content-Type is a multi value field. Values are seperated with the ';' char
					string contentType = httpResponse.HeaderFields[NHttpHeaderFieldName.ContentType];
					string[] contentTypes = contentType.Split(new char[] { ';' });

					// normalize content type values (trim and make lower case)
					for (int i = 0; i < contentTypes.Length; i++)
					{
						contentTypes[i] = contentTypes[i].Trim();
						contentTypes[i] = contentTypes[i].ToLower();
					}

					// the first part of the content type is the mime type of the content
					switch (contentTypes[0])
					{
						case "image/png":
						case "image/jpeg":
						case "image/bmp":
							NImage image = new NImage(new NBytesImageSource(httpResponse.DataArray));
							NImageBox imageBox = new NImageBox(image);
							ResponseContentHolder.Content = new NScrollContent(imageBox);
							break;

						case "text/html":
						case "application/json":
							string charSet = (contentTypes.Length >= 1 ? contentTypes[1] : "charset=utf-8");
							string html = "";
							switch (charSet)
							{
								case "charset=utf-8":
									html = Nevron.Nov.Text.NEncoding.UTF8.GetString(httpResponse.DataArray);
									break;

								default:
									html = Nevron.Nov.Text.NEncoding.UTF8.GetString(httpResponse.DataArray);
									break;
							}

							NTextBox textBox = new NTextBox();
							textBox.Text = html;
							ResponseContentHolder.Content = textBox;
							break;

						default:
							break;
					}
				}
				catch (Exception ex)
				{
					ResponseContentHolder.Content = new NLabel("Request for URI: " + Request.Uri.ToString() + " decoding failed. Error was: " + ex.Message.ToString());
				}
			}
			/// <summary>
			/// Handles a File response
			/// </summary>
			/// <param name="fileResponse"></param>
			private void HandleFileResponse(NFileWebResponse fileResponse)
			{
				string extension = NPath.Current.GetExtension(Request.Uri.GetLocalPath());

				switch (extension)
				{
					case "png":
					case "jpeg":
					case "bmp":
						NImage image = new NImage(new NBytesImageSource(fileResponse.DataArray));
						NImageBox imageBox = new NImageBox(image);
						ResponseContentHolder.Content = new NScrollContent(imageBox);
						break;

					case "html":
					case "json":
					case "txt":
						string html = Nevron.Nov.Text.NEncoding.UTF8.GetString(fileResponse.DataArray);
						NTextBox textBox = new NTextBox();
						textBox.Text = html;
						ResponseContentHolder.Content = textBox;
						break;

					default:
						break;
				}
			}

			#endregion

			#region Fields

			public readonly NContentHolder ResponseContentHolder;
			public readonly NWebRequest Request;
			public readonly NListBoxItem ListBoxItem;
			public readonly NProgressBar ProgressBar;
			public readonly NLabel StatusLabel;
			public readonly NButton AbortButton;
			public readonly NButton ViewHeadersButton;

			public NWebResponse Response;

			#endregion
		}

		#endregion
	}
}