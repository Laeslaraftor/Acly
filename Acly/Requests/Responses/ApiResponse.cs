using Newtonsoft.Json;
using System;

namespace Acly.Requests
{
	/// <summary>
	/// �����, ����������� ����� Api �������
	/// </summary>
	[Serializable]
	public class ApiResponse : Response
	{
		/// <summary>
		/// ������� ��������� ������ �������
		/// </summary>
		public ApiResponse() { }
		/// <summary>
		/// ������� ��������� ������ �������
		/// </summary>
		/// <param name="Status">������ ������</param>
		/// <param name="Code">��� ������</param>
		/// <param name="Text">��������� ������</param>
		public ApiResponse(ApiResponseStatus Status, string Code, string Text)
		{
			this.Status = Status;
			this.Code = Code;
			this.Text = Text;
		}
		/// <summary>
		/// ������� ��������� ������ �������
		/// </summary>
		/// <param name="Response">�����</param>
		/// <param name="Status">������ ������</param>
		/// <exception cref="ArgumentNullException"></exception>
		public ApiResponse(Response Response, ApiResponseStatus Status)
		{
			if (Response == null)
			{
				throw new ArgumentNullException(nameof(Response), "����� �� ������");
			}

			this.Status = Status;
			Code = Response.Code;
			Text = Response.Text;
		}
		/// <summary>
		/// ������� ��������� ������ �������
		/// </summary>
		/// <param name="Exception">���������� ��� ����� �������</param>
		/// <exception cref="ArgumentNullException">���������� �� �������</exception>
		public ApiResponse(Exception Exception) : base(Exception)
		{
		}

		/// <summary>
		/// ������ ������
		/// </summary>
		[JsonProperty("status")]
		public ApiResponseStatus Status { get; set; } = ApiResponseStatus.Error;

		/// <summary>
		/// �������� �� ����� ��������
		/// </summary>
		public bool IsSuccess => Status == ApiResponseStatus.Success;

		/// <summary>
		/// �������������� ����� � ���� ������
		/// </summary>
		/// <returns>����� ��� ���� ������</returns>
		public override string ToString()
		{
			return Status + " | " + Code + " | " + Text;
		}
	}
}