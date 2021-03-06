using DALEFModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class APIProxy
	{
		#region BaseURI
		static string baseUri = string.Empty;
		public static string BaseUri
		{
			get
			{
				baseUri = ConfigurationManager.AppSettings["ApiUrl"];
				return baseUri;
			}

		}
		#endregion

		public List<SelectResult> GetMealPreferences()
		{
			List<SelectResult> mealprefs = new List<SelectResult>();
			GridResult<SelectResult> result = new GridResult<SelectResult>();
			try
			{
				result = PrePopulateData("StpDataID as ID, DataDescription as Description ", "StpData", " IsActive = 'true' AND StpDataTypeID = 33 ");
				mealprefs = result.Items?.ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return mealprefs;
		}


		#region Student Biometric Methods
		public bool UpdateStudentsFromEdAdmin(string school, out string result)
		{
			bool updated = false;
			result = "Success";
			try 
			{ 

				string uri = BaseUri + "StudentController/GetStudentsFromEdAdmin";
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + school).Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						updated = true;
					
					}
					else
					{
						result = content.Result;
					}
				
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return updated;
		}
		public List<SelectResult> GetStudentList(string searchSurname = "")
		{
			List<SelectResult> students = new List<SelectResult>();
			GridResult<SelectResult> result = new GridResult<SelectResult>();
			try
			{
				if (searchSurname.Length > 0)
				{
					result = PrePopulateData("StudentID as ID,Firstname +' ' + Surname + ' ' + isnull(AdmissionNo,'') as Description ", "Student", " Surname like '" + searchSurname + "%' and IsActive = 'true' ");
				}
				else
				{
					result = PrePopulateData("StudentID as ID, Firstname + ' ' + Surname + ' ' + isnull(AdmissionNo, '') as Description ", "Student", " IsActive = 'true' ");
				}
				students = result.Items?.ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return students;
		}

		public StudentBiometric GetStudentBiometric(int id, int biometricType, out string result)
		{
			string uri = BaseUri + "StudentBiometricController/GetStudentBiometricByStudentID";
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
				var content = response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					var settings = new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore,
						MissingMemberHandling = MissingMemberHandling.Ignore
					};
					List<StudentBiometric> items = JsonConvert.DeserializeObject<List<StudentBiometric>>(content.Result, settings);
					result = "Success";
					return items.Where(o => o.Biometric?.StpBiometricTypeID == biometricType).FirstOrDefault(); //Fingerprint biometric
				}
				else
				{
					result = content.Result;
					return null;
				}

			}
		}

		public Student GetStudent(int id,out string result)
		{
			string uri = BaseUri + "StudentController/Student";
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/false").Result;
				var content = response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					var settings = new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore,
						MissingMemberHandling = MissingMemberHandling.Ignore
					};
					Student student = JsonConvert.DeserializeObject<Student>(content.Result, settings);
					result = "Success";
					return student;
				}
				else
				{
					result = content.Result;
					return null;
				}

			}
		}
		public bool EditStudent(Student newItem, out string result)
		{
			bool success = false;
			result = "";
			try
			{

				string uri = BaseUri + "StudentController/Student";
				string uriAdd = BaseUri + "StudentController/Student/add";
				string uriUpdate = BaseUri + "StudentController/Student/update";
				Student itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.StudentID + "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						var settings = new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
						};
						itemExists = JsonConvert.DeserializeObject<Student>(content.Result, settings);
					}
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					//Insert
					if (itemExists == null)
					{
						HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
						var resultAdd = responseAdd.Content.ReadAsStringAsync();
						if (responseAdd.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultAdd.Result;
						}
					}
					else//Update
					{
						HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
						var resultOut = responseOut.Content.ReadAsStringAsync();
						if (responseOut.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultOut.Result;
						}
					}
				}
				return success;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		public List<StudentBiometric> GetStudentBiometricList(int orgID, int biometricType, out string result)
		{
			try { 
			string uri = BaseUri + "StudentBiometricController/GetStudentBiometricList";
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = httpClient.GetAsync(uri + "/" + orgID + "/" + biometricType + "/true").Result;
				var content = response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					var settings = new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore,
						MissingMemberHandling = MissingMemberHandling.Ignore
					};
					List<StudentBiometric> items = JsonConvert.DeserializeObject<List<StudentBiometric>>(content.Result, settings);
					result = "Success";
					return items;
				}
				else
				{
					result = content.Result;
					return null;
				}

			}
			}
			catch (AggregateException ex)
			{
				throw ex;
			}
		}

		public bool EditStudentBiometric(StudentBiometric newItem, out string result)
		{
			bool success = false;
			result = "";
			try
			{

				string uri = BaseUri + "StudentBiometricController/StudentBiometric";
				string uriAdd = BaseUri + "StudentBiometricController/StudentBiometric/add";
				string uriUpdate = BaseUri + "StudentBiometricController/StudentBiometric/update";
				StudentBiometric itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.StudentBiometricID + "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						var settings = new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
						};
						itemExists = JsonConvert.DeserializeObject<StudentBiometric>(content.Result, settings);
					}
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					//Insert
					if (itemExists == null)
					{
						HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
						var resultAdd = responseAdd.Content.ReadAsStringAsync();
						if (responseAdd.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultAdd.Result;
						}
					}
					else//Update
					{
						HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
						var resultOut = responseOut.Content.ReadAsStringAsync();
						if (responseOut.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultOut.Result;
						}
					}
				}
				return success;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		
		public bool EditStudentMeal(StudentMeal newItem, out string result)
		{
			bool success = false;
			result = "";
			try
			{

				string uri = BaseUri + "StudentMealController/StudentMeal";
				string uriAdd = BaseUri + "StudentMealController/StudentMeal/add";
				string uriUpdate = BaseUri + "StudentMealController/StudentMeal/update";
				StudentMeal itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.StudentMealID + "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						var settings = new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
						};
						itemExists = JsonConvert.DeserializeObject<StudentMeal>(content.Result, settings);
					}
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					//Insert
					if (itemExists == null)
					{
						HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
						var resultAdd = responseAdd.Content.ReadAsStringAsync();
						if (responseAdd.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultAdd.Result;
						}
					}
					else//Update
					{
						HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
						var resultOut = responseOut.Content.ReadAsStringAsync();
						if (responseOut.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultOut.Result;
						}
					}
				}
				return success;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		
		#endregion

		#region Person Biometric Methods

		public List<SelectResult> GetPersonList(string searchSurname = "")
		{
			List<SelectResult> students = new List<SelectResult>();
			GridResult<SelectResult> result = new GridResult<SelectResult>();
			try
			{
				if (searchSurname.Length > 0)
				{
					result = PrePopulateData("PersonID as ID,Firstnames +' ' + Surname + ' ' + isnull(Comment,'') as Description ", "Person", " Surname like '" + searchSurname + "%' and IsActive = 'true' ");
				}
				else
				{
					result = PrePopulateData("PersonID as ID,Firstnames +' ' + Surname + ' ' + isnull(Comment,'') as Description ", "Person", " IsActive = 'true' ");
				}
				students = result.Items?.ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return students;
		}
		
		public PersonBiometric GetPersonBiometric(int id,int biometricType, out string result)
		{
			string uri = BaseUri + "PersonBiometricController/PersonBiometricByPersonID";
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
				var content = response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					var settings = new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore,
						MissingMemberHandling = MissingMemberHandling.Ignore
					};
					List<PersonBiometric> items = JsonConvert.DeserializeObject<List<PersonBiometric>>(content.Result, settings);
					result = "Success";
					return items.Where(o=>o.Biometric?.StpBiometricTypeID == biometricType).FirstOrDefault(); //Fingerprint biometric
				}
				else
				{
					result = content.Result;
					return null;
				}

			}
		}

		public List<PersonBiometric> GetPersonBiometricList(int orgID, int biometricType, out string result)
		{
			string uri = BaseUri + "PersonBiometricController/GetBiometricList";
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = httpClient.GetAsync(uri + "/" + orgID + "/" + biometricType + "/true").Result;
				var content = response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					var settings = new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore,
						MissingMemberHandling = MissingMemberHandling.Ignore
					};
					List<PersonBiometric> items = JsonConvert.DeserializeObject<List<PersonBiometric>>(content.Result, settings);
					result = "Success";
					return items;
				}
				else
				{
					result = content.Result;
					return null;
				}

			}
		}

		public bool EditPersonBiometric(PersonBiometric newItem, out string result)
		{
			bool success = false;
			result = "";
			try
			{

				string uri = BaseUri + "PersonBiometricController/PersonBiometric";
				string uriAdd = BaseUri + "PersonBiometricController/PersonBiometric/add";
				string uriUpdate = BaseUri + "PersonBiometricController/PersonBiometric/update";
				PersonBiometric itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.PersonBiometricID + "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						var settings = new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
						};
						itemExists = JsonConvert.DeserializeObject<PersonBiometric>(content.Result, settings);
					}
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					//Insert
					if (itemExists == null)
					{
						HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
						var resultAdd = responseAdd.Content.ReadAsStringAsync();
						if (responseAdd.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultAdd.Result;
						}
					}
					else//Update
					{
						HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
						var resultOut = responseOut.Content.ReadAsStringAsync();
						if (responseOut.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultOut.Result;
						}
					}
				}
				return success;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		public bool EditBiometric(Biometric newItem, out string result)
		{
			bool success = false;
			result = "";
			try
			{

				string uri = BaseUri + "BiometricController/Biometric";
				string uriAdd = BaseUri + "BiometricController/Biometric/add";
				string uriUpdate = BaseUri + "BiometricController/Biometric/update";
				Biometric itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.BiometricID + "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						var settings = new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
						};
						itemExists = JsonConvert.DeserializeObject<Biometric>(content.Result, settings);
					}
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					//Insert
					if (itemExists == null)
					{
						HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
						var resultAdd = responseAdd.Content.ReadAsStringAsync();
						if (responseAdd.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultAdd.Result;
						}
					}
					else//Update
					{
						HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
						var resultOut = responseOut.Content.ReadAsStringAsync();
						if (responseOut.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultOut.Result;
						}
					}
				}
				return success;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		public bool EditAttendance(Attendance newItem, out string result)
		{
			bool success = false;
			result = "";
			try
			{

				string uri = BaseUri + "AttendanceController/Attendance";
				string uriAdd = BaseUri + "AttendanceController/Attendance/add";
				string uriUpdate = BaseUri + "AttendanceController/Attendance/update";
				Attendance itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.AttendanceID + "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						var settings = new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
						};
						itemExists = JsonConvert.DeserializeObject<Attendance>(content.Result, settings);
					}
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					//Insert
					if (itemExists == null)
					{
						HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
						var resultAdd = responseAdd.Content.ReadAsStringAsync();
						if (responseAdd.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultAdd.Result;
						}
					}
					else//Update
					{
						HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
						var resultOut = responseOut.Content.ReadAsStringAsync();
						if (responseOut.IsSuccessStatusCode)
						{
							success = true;
						}
						else
						{
							success = false;
							result = resultOut.Result;
						}
					}
				}
				return success;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		//public Person GetPerson(int id, out string result)
		//{
		//	string uri = BaseUri + "PersonController/Person";
		//	using (HttpClient httpClient = new HttpClient())
		//	{
		//		httpClient.DefaultRequestHeaders.Accept.Clear();
		//		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		//		HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
		//		var content = response.Content.ReadAsStringAsync();
		//		if (response.IsSuccessStatusCode)
		//		{
		//			var settings = new JsonSerializerSettings
		//			{
		//				NullValueHandling = NullValueHandling.Ignore,
		//				MissingMemberHandling = MissingMemberHandling.Ignore
		//			};
		//			Person item = JsonConvert.DeserializeObject<Person>(content.Result, settings);
		//			result = "Success";
		//			return item;
		//		}
		//		else
		//		{
		//			result = content.Result;
		//			return null;
		//		}

		//	}
		//}
		#endregion

		public GridResult<SelectResult> PrePopulateData(string field, string table, string where = "", string orderby = "", string direction = "ASC")
		{
			try
			{

				GridResult<SelectResult> dataList = null;
				
				if (field.Length > 0 && table.Length > 0)
				{
					string uri = BaseUri + "AdminController/GetSelectList";
					////Filter by users org Note only display data with in users organization
					where = where + " and OrgID = " + ConfigurationManager.AppSettings["OrgID"];//dsg
					SelectQuery qry = new SelectQuery() { fields = field, table = table, where = where, orderby = orderby, direction = direction };
					using (HttpClient httpClient = new HttpClient())
					{
						httpClient.DefaultRequestHeaders.Accept.Clear();
						httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						StringContent content = new StringContent(JsonConvert.SerializeObject(qry), Encoding.UTF8, "application/json");
						HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
						var result = response.Content.ReadAsStringAsync();
						if (response.IsSuccessStatusCode)
						{
							var settings = new JsonSerializerSettings
							{
								NullValueHandling = NullValueHandling.Ignore,
								MissingMemberHandling = MissingMemberHandling.Ignore
							};
							dataList = JsonConvert.DeserializeObject<GridResult<SelectResult>>(result.Result, settings);
						}
						else
						{
							throw new Exception(result.Result);
						}

					}

				}
				return dataList;


			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
