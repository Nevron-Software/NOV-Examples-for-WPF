using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NAccordionFirstLookExample : NExampleBase
	{
		#region Constructors

		public NAccordionFirstLookExample()
		{
		}

		static NAccordionFirstLookExample()
		{
			NAccordionFirstLookExampleSchema = NSchema.Create(typeof(NAccordionFirstLookExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
            // create an accordion
			m_Accordion = new NAccordion();
			m_Accordion.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_Accordion.VerticalPlacement = ENVerticalPlacement.Top;
			m_Accordion.MinWidth = 300;
            m_Accordion.SetBorder(1, NColor.Red);

            // create a stack panel to hold the expandable sections of the accordion
            // note that the accordion is designed like a radio button group, allowing the user to use any layout 
            // to arrange the expandable sections managed by the accordion.
            NStackPanel stack = new NStackPanel();
            stack.VerticalSpacing = 0;
            m_Accordion.Content = stack;

            // create the sections
            stack.Add(CreateAccordionSection(NResources.Image__16x16_Mail_png, "Mail", CreateMailTreeView(), false));
            stack.Add(CreateAccordionSection(NResources.Image__16x16_Calendar_png, "Calendar", CreateCalendar(), true));
            stack.Add(CreateAccordionSection(NResources.Image__16x16_Contacts_png, "Contacts", CreateContactsTreeView(), false));
            stack.Add(CreateAccordionSection(NResources.Image__16x16_Tasks_png, "Tasks", CreateTasksView(), false));

			return m_Accordion;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;		

			// properties
			NProperty[] properties = new NProperty[] { NAccordion.ShowSymbolProperty };
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_Accordion).CreatePropertyEditors(m_Accordion, properties);
			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}		

			// create the events list box
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);
			
			return stack;			
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use the Accordion widget. Red border is set to the accordion
    so that you can see what is its size when a specific section is expanded.
</p>
";
		}

		#endregion

        #region Implementation

        /// <summary>
        /// Creates an accordion section.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        /// <param name="content"></param>
        /// <param name="expanded"></param>
        /// <returns></returns>
        private NExpandableSection CreateAccordionSection(NImage image, string text, NWidget content, bool expanded)
        {
            NPairBox header = NPairBox.Create(image, text);
            NExpandableSection section = new NExpandableSection(header, content);
            section.Expanded = expanded;

            return section;
        }

        /// <summary>
        /// Creates a dummy tree view, that contains the items of an imaginary Mail
        /// </summary>
        /// <returns></returns>
        private NTreeView CreateMailTreeView()
        {
            NTreeView treeView = new NTreeView();
            NTreeViewItem rootItem = CreateTreeViewItem("Personal Folers", NResources.Image__16x16_folderHome_png);
            treeView.Items.Add(rootItem);
            string[] texts = new string[] {
                "Deleted Items", 
				"Drafts", 
				"Inbox", 
				"Junk E-mails", 
				"Outbox", 
				"RSS Feeds", 
				"Sent Items", 
				"Search Folders"
            };

            NImage[] icons = new NImage[] { 
				NResources.Image__16x16_folderDeleted_png,
				NResources.Image__16x16_folderDrafts_png,
				NResources.Image__16x16_folderInbox_png,
				NResources.Image__16x16_folderJunk_png,
				NResources.Image__16x16_folderOutbox_png,
				NResources.Image__16x16_folderRss_png,
				NResources.Image__16x16_folderSent_png,
                NResources.Image__16x16_folderSearch_png
			};

            for (int i = 0; i < texts.Length; i++)
            {
                rootItem.Items.Add(CreateTreeViewItem(texts[i], icons[i]));
            }

            treeView.ExpandAll(true);
            treeView.BorderThickness = new NMargins(0);

            return treeView;
        }
        /// <summary>
        /// Creates the Contacts tree view
        /// </summary>
        /// <returns></returns>
        private NTreeView CreateContactsTreeView()
        {
            string[] names = new string[] { "Emily Swan", "John Smith", "Lindsay Collier", "Kevin Johnson", "Shannon Flynn" };
            return SetupTreeView(names, NResources.Image__16x16_Contacts_png);
        }
        /// <summary>
        /// Creates a dummy callendar for the Calendar section.
        /// </summary>
        /// <returns></returns>
        private NCalendar CreateCalendar()
        {
            NCalendar calendar = new NCalendar();
            calendar.Margins = new NMargins(10);
            calendar.BorderThickness = new NMargins(0);
            return calendar;
        }
        /// <summary>
        /// Creates the Tasks tree view
        /// </summary>
        /// <returns></returns>
        private NTreeView CreateTasksView()
        {
            string[] tasks = new string[] { "Meet John", "Montly report", "Pickup kids", "Make backup" };
            return SetupTreeView(tasks, NResources.Image__16x16_Tasks_png);
        }
        
        private NTreeView SetupTreeView(string[] texts, NImage image)
        {
            NTreeView treeView = new NTreeView();
            treeView.BorderThickness = new NMargins(0);
            for (int i = 0; i < texts.Length; i++)
            {
                treeView.Items.Add(CreateTreeViewItem(texts[i], (NImage)image.DeepClone()));
            }

            return treeView;
        }
        /// <summary>
        /// Creates a tree view item.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        private NTreeViewItem CreateTreeViewItem(string text, NImage icon)
        {
            NStackPanel stack = new NStackPanel();
            stack.Direction = ENHVDirection.LeftToRight;
            stack.HorizontalSpacing = 3;

            if (icon != null)
            {
                NImageBox imageBox = new NImageBox(icon);
                imageBox.HorizontalPlacement = ENHorizontalPlacement.Center;
                imageBox.VerticalPlacement = ENVerticalPlacement.Center;

                stack.Add(imageBox);
            }

            if (!string.IsNullOrEmpty(text))
            {
                NLabel label = new NLabel(text);
                label.VerticalPlacement = ENVerticalPlacement.Center;
                stack.Add(label);
            }

            NTreeViewItem item = new NTreeViewItem(stack);
            item.Margins = new NMargins(0, 5);

            return item;
        }

        #endregion

        #region Fields

        private NExampleEventsLog m_EventsLog;
		private NAccordion m_Accordion;

		#endregion

		#region Schema

		public static readonly NSchema NAccordionFirstLookExampleSchema;

		#endregion
	}
}