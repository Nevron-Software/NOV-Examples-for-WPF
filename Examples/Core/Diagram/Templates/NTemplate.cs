using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Diagram;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples.Diagram
{
	/// <summary>
	/// The NTemplate abstract class serves as base class for all programmable templates
	/// </summary>
	public abstract class NTemplate
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NTemplate()
		{
			Initialize("Template");
		}

		/// <summary>
		/// Initializer contructor
		/// </summary>
		/// <param name="name">name of the template</param>
		public NTemplate(string name)
		{
			Initialize(name);
		}

		#endregion

		#region Events

		/// <summary>
		/// Fired when the template has been changed
		/// </summary>
		public event EventHandler TemplateChanged;

		#endregion

		#region Properties

		/// <summary>
		/// Gets/set the template node
		/// </summary>
		public string Name
		{
			get
			{
				return m_sName;
			}
			set
			{
				if (value == null)
					throw new NullReferenceException();

				if (value == m_sName)
					return;

				m_sName = value;
				OnTemplateChanged();
			}
		}

		/// <summary>
		/// Gets or sets the template origin
		/// </summary>
		/// <remarks>
		/// The origin defines the location at which the template will be instanciated in the document
		/// </remarks>
		public NPoint Origin
		{
			get
			{
				return m_Origin;
			}
			set
			{
				if (value == m_Origin)
					return;

				m_Origin = value;
				OnTemplateChanged();
			}
		}

		/// <summary>
		/// Specifies the history transaction description 
		/// </summary>
		public string TransactionDescription
		{
			get
			{
				return m_sTransactionDescription;
			}
			set
			{
				if (value == m_sTransactionDescription)
					return;

				if (value == null)
					throw new NullReferenceException();

				m_sTransactionDescription = value;
			}
		}

		
		#endregion
        
		#region Overridable

		/// <summary>
		/// Creates the template in the specified document
		/// </summary>
		/// <remarks>
		/// This method will call the CreateTemplate method. 
		/// The call will be embraced in a transaction with the specified TransactionDescription
		/// </remarks>
		/// <param name="document">document in which to create the template</param>
		/// <returns>true if the template was successfully created, otherwise false</returns> 
		public virtual bool Create(NDrawingDocument document)
		{
			if (document == null)
				throw new ArgumentNullException("document");

			document.StartHistoryTransaction(m_sTransactionDescription);

			try
			{
				CreateTemplate(document);
			}
			catch (Exception ex)
			{
				NTrace.WriteLine("Failed to create template. Exception was: " + ex.Message);
				document.RollbackHistoryTransaction();
				return false;
			}

			document.CommitHistoryTransaction();
			return true;
		}

		/// <summary>
		/// Obtains a dynamic human readable description of the template
		/// </summary>
		/// <returns>template description</returns>
		public abstract string GetDescription();
	
		#endregion

		#region Protected overridable

		/// <summary>
		/// Must override to create the template
		/// </summary>
		/// <param name="document">document in which to create the template</param>
		protected abstract void CreateTemplate(NDrawingDocument document);
		/// <summary>
		/// Called when the template has changed
		/// </summary>
		/// <remarks>
		/// This implementation will raise the TemplateChanged event
		/// </remarks>
		protected virtual void OnTemplateChanged()
		{
			if (TemplateChanged != null)
				TemplateChanged(this, null);
		}
		
		#endregion

		#region Private

		private void Initialize(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name"); 

			// props
			m_sTransactionDescription = "Create Template";
			m_sName = name;
			m_Origin = new NPoint();
		}


		#endregion

		#region Fields

		internal string m_sName;
		internal string m_sTransactionDescription;
		internal NPoint m_Origin;

		#endregion
	}
}
