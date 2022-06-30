using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;

namespace Nevron.Nov.Examples.UI
{
    public class NNavigationBarFirstLookExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NNavigationBarFirstLookExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NNavigationBarFirstLookExample()
        {
            NNavigationBarFirstLookExampleSchema = NSchema.Create(typeof(NNavigationBarFirstLookExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // Create a list box
            m_NavigationBar = new NNavigationBar();
            m_NavigationBar.HorizontalPlacement = ENHorizontalPlacement.Left;
            m_NavigationBar.VerticalPlacement = ENVerticalPlacement.Fit;

            // create the Outlook Bar Panes
            CreateNavigationBarPane("Mail", "Mail", NResources.Image__24x24_Mail_png, NResources.Image__16x16_Mail_png, new NLabel("Mail Content"));
            CreateNavigationBarPane("Calendar", "Calendar", NResources.Image__24x24_Calendar_png, NResources.Image__16x16_Calendar_png, new NLabel("Calendar Content"));
            CreateNavigationBarPane("Contacts", "Contacts", NResources.Image__24x24_Contacts_png, NResources.Image__16x16_Contacts_png, new NLabel("Contacts Content"));
            CreateNavigationBarPane("Tasks", "Tasks", NResources.Image__24x24_Tasks_png, NResources.Image__16x16_Tasks_png, new NLabel("Tasks Content"));
            CreateNavigationBarPane("Notes", "Notes", NResources.Image__24x24_Notes_png, NResources.Image__16x16_Notes_png, new NLabel("Notes Content"));
            CreateNavigationBarPane("Folders", "Folders", NResources.Image__24x24_Folders_png, NResources.Image__16x16_Folders_png, new NLabel("Folders Content"));
            CreateNavigationBarPane("Shortcuts", "Shortcuts", NResources.Image__24x24_Shortcuts_png, NResources.Image__16x16_Shortcuts_png, new NLabel("Shortcuts Content"));

            // Hook to list box selection events
            m_NavigationBar.SelectedIndexChanged += OnNavigationBarSelectedIndexChanged;

            return m_NavigationBar;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            stack.FillMode = ENStackFillMode.Last;
            stack.FitMode = ENStackFitMode.Last;

            // Create the properties group box
            stack.Add(CreatePropertiesGroupBox());

            // Create the events log
            m_EventsLog = new NExampleEventsLog();
            stack.Add(m_EventsLog);

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
	This example demonstrates how to create a simple NavigationBar and populate it with items. You can use the controls
	on the right to modify various properties of the NavigationBar.
</p>
";
        }

        private NNavigationBarPane CreateNavigationBarPane(string title, string tooltip, NImage largeImage, NImage smallImage, NWidget content)
        {
            NNavigationBarPane pane = new NNavigationBarPane();

            // set pane content
            pane.Content = content;
            pane.Image = (NImage)smallImage.DeepClone();
            pane.Text = title;

            // set header content
            NLabel titleLabel = new NLabel(title);
            titleLabel.VerticalPlacement = ENVerticalPlacement.Fit;
            
            NPairBox headerContent = new NPairBox(largeImage, titleLabel);
            headerContent.Spacing = 2;
            pane.Header.Content = headerContent;
            pane.Header.Tooltip = new NTooltip(tooltip);

            // set icon content
            pane.Icon.Content = new NImageBox(smallImage);
            pane.Icon.Tooltip = new NTooltip(tooltip);

            // add the pane
            m_NavigationBar.Panes.Add(pane);
            return pane;
        }
        private void OnNavigationBarSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_EventsLog.LogEvent("Selected Index " + m_NavigationBar.SelectedIndex);
        }

        #endregion

        #region Implementation

        private NGroupBox CreatePropertiesGroupBox()
        {
            NStackPanel propertiesStack = new NStackPanel();

            NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_NavigationBar).CreatePropertyEditors(m_NavigationBar,
                NNavigationBar.EnabledProperty,
                NNavigationBar.HorizontalPlacementProperty,
                NNavigationBar.VerticalPlacementProperty,
                NNavigationBar.VisibleHeadersCountProperty,
                NNavigationBar.HeadersPaddingProperty,
                NNavigationBar.HeadersSpacingProperty,
                NNavigationBar.IconsPaddingProperty,
                NNavigationBar.IconsSpacingProperty
            );

            for (int i = 0, count = editors.Count; i < count; i++)
            {
                propertiesStack.Add(editors[i]);
            }

            NGroupBox propertiesGroupBox = new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack));
            return propertiesGroupBox;
        }

        #endregion

        #region Event Handlers

        #endregion

        #region Fields

        private NNavigationBar m_NavigationBar;
        private NExampleEventsLog m_EventsLog;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NNavigationBarFirstLookExample.
        /// </summary>
        public static readonly NSchema NNavigationBarFirstLookExampleSchema;

        #endregion
    }
}