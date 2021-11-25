using System;

namespace Futronic.SDK.WorkedEx
{
	/// <summary>
	/// Summary description for ComboBoxItem.
	/// </summary>
	public class ComboBoxItem
	{
		public ComboBoxItem(String message, Object tag)
		{
			m_Message = message;
			m_Tag = tag;
		}

		public String Message
		{
			get
			{
				return m_Message;
			}
		}

		public Object Tag
		{
			get
			{
				return m_Tag;
			}
		}

		public override string ToString()
		{
			return m_Message;
		}

		protected String m_Message;
		protected Object m_Tag;
	}
}
