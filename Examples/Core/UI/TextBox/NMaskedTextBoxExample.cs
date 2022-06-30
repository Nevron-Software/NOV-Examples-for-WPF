using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
    public class NMaskedTextBoxExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NMaskedTextBoxExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NMaskedTextBoxExample()
        {
            NMaskedTextBoxExampleSchema = NSchema.Create(typeof(NMaskedTextBoxExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            NStackPanel stack = new NStackPanel();
            stack.HorizontalPlacement = ENHorizontalPlacement.Left;

            m_PhoneNumberTextBox = new NMaskedTextBox();
            m_PhoneNumberTextBox.Mask = "(999) 000-0000";
            m_PhoneNumberTextBox.MaskInputRejected += OnMaskInputRejected;
            stack.Add(NPairBox.Create("Phone Number:", m_PhoneNumberTextBox));

            m_CreditCardNumberTextBox = new NMaskedTextBox();
            m_CreditCardNumberTextBox.Mask = "0000 0000 0000 0000";
            m_CreditCardNumberTextBox.MaskInputRejected += OnMaskInputRejected;
            stack.Add(NPairBox.Create("Credit Card Number:", m_CreditCardNumberTextBox));

            m_SocialSecurityNumberTextBox = new NMaskedTextBox();
            m_SocialSecurityNumberTextBox.Mask = "000-00-0000";
            m_SocialSecurityNumberTextBox.MaskInputRejected += OnMaskInputRejected;
            stack.Add(NPairBox.Create("Social Security Number:", m_SocialSecurityNumberTextBox));

            return new NUniSizeBoxGroup(stack);
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            stack.FillMode = ENStackFillMode.Last;
            stack.FitMode = ENStackFitMode.Last;

            NTextBox promptCharTextBox = new NTextBox("_");
            promptCharTextBox.MaxLength = 1;
            promptCharTextBox.SelectAllOnFocus = true;
            promptCharTextBox.HorizontalPlacement = ENHorizontalPlacement.Left;
            promptCharTextBox.TextChanged += OnPromptCharTextBoxTextChanged;
            stack.Add(NPairBox.Create("Prompt char: ", promptCharTextBox));

            m_EventsLog = new NExampleEventsLog();
            stack.Add(m_EventsLog);

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    This example demonstrates how to create, configure and use masked text boxes. The text box on the right lets you configure
    the prompt character (i.e. the <b>PromptChar</b> property) of the sample masked text boxes.
</p>
";
        }

        #endregion

        #region Event Handlers

        private void OnMaskInputRejected(NMaskInputRejectedEventArgs arg)
        {
            m_EventsLog.LogEvent("Rejected Character: '" + arg.Character + "', reason: '" +
                NStringHelpers.InsertSpacesBeforeUppersAndDigits(arg.Reason.ToString()) + "'");
        }
        private void OnPromptCharTextBoxTextChanged(NValueChangeEventArgs arg)
        {
            string text = (string)arg.NewValue;

            if (text != null && text.Length == 1)
            {
                char promptChar = text[0];
                m_PhoneNumberTextBox.PromptChar = promptChar;
                m_CreditCardNumberTextBox.PromptChar = promptChar;
                m_SocialSecurityNumberTextBox.PromptChar = promptChar;
            }
        }

        #endregion

        #region Fields

        private NMaskedTextBox m_PhoneNumberTextBox;
        private NMaskedTextBox m_CreditCardNumberTextBox;
        private NMaskedTextBox m_SocialSecurityNumberTextBox;

        private NExampleEventsLog m_EventsLog;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NMaskedTextBoxExample.
        /// </summary>
        public static readonly NSchema NMaskedTextBoxExampleSchema;

        #endregion
    }
}
