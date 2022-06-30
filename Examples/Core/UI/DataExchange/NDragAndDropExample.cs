using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NDragAndDropExample : NExampleBase
	{
		#region Constructors

		public NDragAndDropExample()
		{
		}
		static NDragAndDropExample()
		{
			NDragAndDropExampleSchema = NSchema.Create(typeof(NDragAndDropExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// sources
			NGroupBox sourcesGroup = new NGroupBox("Drag Drop Sources");
			{
				NStackPanel sourcesStack = new NStackPanel();
				sourcesGroup.Content = sourcesStack;

				NContentHolder textSource1 = CreateDemoElement("Drag Source Text 1");
				NDataObject dataObject1 = new NDataObject();
				dataObject1.SetData(NDataFormat.TextFormatString, "Text string 1");
				textSource1.Tag = dataObject1;
				sourcesStack.Add(textSource1);
				textSource1.MouseDown += new Function<NMouseButtonEventArgs>(OnSourceMouseDown);

				NContentHolder textSource2 = CreateDemoElement("Drag Source Text 2");
				NDataObject dataObject2 = new NDataObject();
				dataObject2.SetData(NDataFormat.TextFormatString, "Text string 2");
				textSource2.Tag = dataObject2;
				sourcesStack.Add(textSource2);
				textSource2.MouseDown += new Function<NMouseButtonEventArgs>(OnSourceMouseDown);
			}

			// targets
			NGroupBox targetsGroup = new NGroupBox("Drop Targets");
			{
				NStackPanel targetsStack = new NStackPanel();
				targetsGroup.Content = targetsStack;

				NContentHolder dropTextTarget = CreateDemoElement("Drop Text On Me");
				targetsStack.Add(dropTextTarget);

				dropTextTarget.DragOver += new Function<NDragActionEventArgs>(OnDragOverTextTarget);
				dropTextTarget.DragDrop += new Function<NDragActionEventArgs>(OnDragDropTextTarget);
			}

			// create the source and targets splitter
			NSplitter splitter = new NSplitter();
			splitter.SplitMode = ENSplitterSplitMode.Proportional;
			splitter.SplitFactor = 0.5d;
			splitter.Pane1.Content = sourcesGroup;
			splitter.Pane2.Content = targetsGroup;

			// create the inspector on the bottom
			NGroupBox inspectorGroup = new NGroupBox("Data Object Ispector");

			NListBox inspector = new NListBox();
			inspectorGroup.Content = inspector;

			inspector.DragEnter += new Function<NDragOverChangeEventArgs>(OnInspectorDragEnter);
			inspector.DragLeave += new Function<NDragOverChangeEventArgs>(OnInspectorDragLeave);

			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Fit;
			stack.VerticalPlacement = ENVerticalPlacement.Fit;
			stack.FillMode = ENStackFillMode.Last;
			stack.Add(splitter);
			stack.Add(inspectorGroup);

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NTab tab = new NTab();

			m_SourceEventsLog = new NExampleEventsLog();
			tab.TabPages.Add(new NTabPage("Source Events", m_SourceEventsLog));

			m_TargetEventsLog = new NExampleEventsLog();
			tab.TabPages.Add(new NTabPage("Target Events", m_TargetEventsLog));

			return tab;
		}

		protected override string GetExampleDescription()
		{
            return @"<p>The example demonstrates how to use Drag and Drop in NOV.</p>";
		}

		#endregion

		#region Implementation

		private NWidget CreateTextDragDropSources()
		{
			return null;
		}
		private NWidget CreateImageDragDropSource()
		{
			return null;
		}
		private NContentHolder CreateDemoElement(string text)
		{
			NContentHolder element = new NContentHolder(text);
			element.Border = NBorder.CreateFilledBorder(NColor.Black, 2, 5);
			element.BorderThickness = new NMargins(1);
			element.BackgroundFill = new NColorFill(NColor.PapayaWhip);
			element.TextFill = new NColorFill(NColor.Black);
			element.Padding = new NMargins(1);
			return element;
		}

		#endregion

		#region Event Handlers

		private void OnSourceMouseDown(NMouseButtonEventArgs args)
		{
			if (NDragDrop.CanRequestDragDrop())
			{
				NDataObject dataObject = (NDataObject)args.CurrentTargetNode.Tag;

				NDragDropSource dropSource = new NDragDropSource(ENDragDropEffects.All);
				dropSource.DragStarting += new Function<NDragDropSourceEventArgs>(OnDropSourceDragStarting);
				dropSource.DragEnded += new Function<NDragEndedEventArgs>(OnDropSourceDragEnded);
				dropSource.QueryDragAction += new Function<NQueryDragActionEventArgs>(OnDropSourceQueryDragAction);

				NDragDrop.RequestDragDrop(dropSource, dataObject);
			}
		}
		private void OnDragOverTextTarget(NDragActionEventArgs args)
		{
			// first you need to check whether the data object is of interest
			if (args.DataObject.ContainsData(NDataFormat.TextFormatString))
			{
				// if the Ctrl is pressed, you must typically copy the data if you can.
				if (NKeyboard.DefaultCommandPressed & (args.AllowedEffect & ENDragDropEffects.Copy) == ENDragDropEffects.Copy)
				{
					args.Effect = ENDragDropEffects.Copy;
					return;
				}

				// if the Alt is pressed, you must typically link the data if you can.
				if (NKeyboard.AltPressed & (args.AllowedEffect & ENDragDropEffects.Link) == ENDragDropEffects.Link)
				{
					args.Effect = ENDragDropEffects.Link;
					return;
				}

				// by default you need to move data if you can
				if ((args.AllowedEffect & ENDragDropEffects.Move) != ENDragDropEffects.None)
				{
					args.Effect = ENDragDropEffects.Move;
					return;
				}
			}

			// the source either did not publish a data object with a format we are interested in,
			// or we cannot perform the action that is standartly performed for the current keyboard modifies state,
			// or the source did not allow the action that we can perfrom for the current keyboard modifies state.
			// in all these cases we set the effect to none.
			args.Effect = ENDragDropEffects.None;
			return;
		}
		private void OnDragDropTextTarget(NDragEventArgs args)
		{
			object data = args.DataObject.GetData(NDataFormat.TextFormatString);
			if (data != null)
			{
				NContentHolder contentHolder = args.CurrentTargetNode as NContentHolder;
				contentHolder.Content = new NLabel("Dropped Text:{" + (string)data + "}. Drop another text...");
			}
		}

		private void OnInspectorDragEnter(NDragOverChangeEventArgs args)
		{
			NDataFormat[] formats = args.DataObject.GetFormats();

			NListBox inspector = args.CurrentTargetNode as NListBox;
			inspector.Items.Clear();

			for (int i = 0; i < formats.Length; i++)
			{
				inspector.Items.Add(new NListBoxItem(formats[i].ToString()));
			}
		}
		private void OnInspectorDragLeave(NDragOverChangeEventArgs args)
		{
			NListBox inspector = args.CurrentTargetNode as NListBox;
			inspector.Items.Clear();
		}

		private void OnDropSourceQueryDragAction(NQueryDragActionEventArgs args)
		{
			m_SourceEventsLog.LogEvent("QueryDragAction " + args.Reason.ToString());
		}
		private void OnDropSourceDragStarting(NDragDropSourceEventArgs args)
		{
			m_SourceEventsLog.LogEvent("DragStarting");
		}
		private void OnDropSourceDragEnded(NDragEndedEventArgs args)
		{
			m_SourceEventsLog.LogEvent("DragEnded. Final Effect was: " + args.FinalEffect.ToString());
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_SourceEventsLog;
		private NExampleEventsLog m_TargetEventsLog;

		#endregion

		#region Schema

		public static readonly NSchema NDragAndDropExampleSchema;

		#endregion
	}
}