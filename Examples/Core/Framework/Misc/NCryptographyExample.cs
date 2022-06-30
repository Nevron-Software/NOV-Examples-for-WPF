using Nevron.Nov.Cryptography;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NCryptographyExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NCryptographyExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NCryptographyExample()
        {
            NCryptographyExampleSchema = NSchema.Create(typeof(NCryptographyExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_DecryptedTextBox = new NTextBox("This is some text to encrypt.");
            m_DecryptedTextBox.PreferredWidth = 200;
            m_DecryptedTextBox.Multiline = true;
            NGroupBox groupBox1 = new NGroupBox("Decrypted Text", m_DecryptedTextBox);
            groupBox1.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);

            m_EncryptedTextBox = new NTextBox();
            m_EncryptedTextBox.PreferredWidth = 200;
            m_EncryptedTextBox.Multiline = true;
            NGroupBox groupBox2 = new NGroupBox("Encrypted Text", m_EncryptedTextBox);
            groupBox2.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);

            NPairBox pairBox = new NPairBox(groupBox1, groupBox2, ENPairBoxRelation.Box1BeforeBox2);
            pairBox.FitMode = ENStackFitMode.Equal;
            pairBox.FillMode = ENStackFillMode.Equal;
            pairBox.Spacing = NDesign.HorizontalSpacing;
            pairBox.PreferredHeight = 300;
            pairBox.VerticalPlacement = ENVerticalPlacement.Top;

            return pairBox;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            
            m_PasswordTextBox = new NTextBox("password");
            stack.Add(NPairBox.Create("Password", m_PasswordTextBox));

            NButton encryptButton = new NButton("Encrypt");
            encryptButton.Click += OnEncryptButtonClick;
            stack.Add(encryptButton);

            return stack;
        }

        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates how to encrypt and decrypt messages using the PK ZIP Classic encryption algorithm. Enter some text in the <b>Decrypted Text</b> text box and
    click the <b>Encrypt</b> button on the right to enrypt it. Then click the <b>Decrypt Button</b> to decrypt it.
</p>
";
        }

        #endregion

        #region Event Handlers

        private void OnEncryptButtonClick(NEventArgs arg)
        {
            NButton button = (NButton)arg.CurrentTargetNode;
            NLabel label = (NLabel)button.Content;

            if (label.Text == "Encrypt")
            {
				m_EncryptedTextBox.Text = NCryptography.EncryptPkzipClassic(m_DecryptedTextBox.Text, m_PasswordTextBox.Text);
                m_DecryptedTextBox.Text = null;
                label.Text = "Decrypt";
            }
            else
            {
				m_DecryptedTextBox.Text = NCryptography.DecryptPkzipClassic(m_EncryptedTextBox.Text, m_PasswordTextBox.Text);
				m_EncryptedTextBox.Text = null;
                label.Text = "Encrypt";
            }
        }

        #endregion

        #region Fields

        private NTextBox m_DecryptedTextBox;
        private NTextBox m_PasswordTextBox;
        private NTextBox m_EncryptedTextBox;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NCryptographyExample.
        /// </summary>
        public static readonly NSchema NCryptographyExampleSchema;

        #endregion
    }
}
