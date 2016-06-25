using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Web;


namespace KAVE.BaseEngine
{
   public static class SMSNeuron
    {
       /// <summary>
       /// Send sms using Bulk SMS Gateway
       /// </summary>
       /// <param name="username">Bulk sms gateway username</param>
        /// <param name="password">Bulk sms gateway password</param>
       /// <param name="msisdn">Phone number with country code without (00 or +)</param>
       /// <param name="msg">your message maximum characters is 160</param>
      public static void SendBulkSMS(string username, string password, string msisdn, string msg)
       {
           try
           {
               if (msg.Length < 160)
               {
                   Hashtable result;
                   string data = seven_bit_message(username, password, msisdn, msg);
                   result = send_sms(data, "http://bulksms.vsms.net:5567/eapi/submission/send_sms/2/2.0");
               }
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }

    

      #region Bulk SMS API
      static string formatted_server_response( Hashtable result ) {
			string ret_string = "";
			if( (int)result["success"] == 1 ) {
				ret_string += "Success: batch ID " + (string)result["api_batch_id"] + "API message: " + (string)result["api_message"] + "\nFull details " + (string)result["details"];
			}
			else {
				ret_string += "Fatal error: HTTP status " + (string)result["http_status_code"] + " API status " + (string)result["api_status_code"] + " API message " + (string)result["api_message"] + "\nFull details " + (string)result["details"];
			}

			return ret_string;
		}

		static Hashtable send_sms( string data, string url ) {
			string sms_result = Post(url, data);
			
			Hashtable result_hash = new Hashtable();

			string tmp = "";
			tmp += "Response from server: " + sms_result + "\n"; 
			string[] parts = sms_result.Split('|');

			string statusCode = parts[0];
			string statusString = parts[1];

			result_hash.Add("api_status_code", statusCode);
			result_hash.Add("api_message", statusString);

			if(parts.Length != 3) {
				tmp += "Error: could not parse valid return data from server.\n"; 
			}
			else {
				if ( statusCode.Equals("0") ) {
					result_hash.Add("success", 1);
					result_hash.Add("api_batch_id", parts[2]);
					tmp += "Message sent - batch ID " + parts[2] + "\n";
				}
				else if (statusCode.Equals("1")) {
					// Success: scheduled for later sending.
					result_hash.Add("success", 1);
					result_hash.Add("api_batch_id", parts[2]);
				}
				else {
					result_hash.Add("success", 0);
					tmp += "Error sending: status code " + parts[0] + " description: " + parts[1] + "\n";
				}
			}
			result_hash.Add("details", tmp);
			return result_hash;
		}

		static string Post(string url, string data) {

			string result = null;
			try {
				byte[] buffer = Encoding.Default.GetBytes(data);

				HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url);
				WebReq.Method = "POST";
				WebReq.ContentType = "application/x-www-form-urlencoded";
				WebReq.ContentLength = buffer.Length;
				Stream PostData = WebReq.GetRequestStream();

				PostData.Write(buffer, 0, buffer.Length);
				PostData.Close();
				HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
				Stream Response = WebResp.GetResponseStream();
				StreamReader _Response = new StreamReader(Response);
				result =  _Response.ReadToEnd();
			}
			catch {
			
			}
			return result.Trim()+"\n";
		}



		static string seven_bit_message( string username, string password, string msisdn, string message ) {
			
			string data = "";
			data += "username="  + HttpUtility.UrlEncode(username, System.Text.Encoding.GetEncoding("ISO-8859-1"));
			data += "&password=" + HttpUtility.UrlEncode(password, System.Text.Encoding.GetEncoding("ISO-8859-1"));
			data += "&message="  + HttpUtility.UrlEncode(message, System.Text.Encoding.GetEncoding("ISO-8859-1"));
			data += "&msisdn=" + msisdn;
			data += "&want_report=1";

			return data;
		}

		static string unicode_message( string username, string password, string msisdn, string message ) {
			
			
			string data = "";
			data += "username="  + HttpUtility.UrlEncode(username, System.Text.Encoding.GetEncoding("ISO-8859-1"));
			data += "&password=" + HttpUtility.UrlEncode(password, System.Text.Encoding.GetEncoding("ISO-8859-1"));
			data += "&message="  + stringToHex( message );
			data += "&msisdn=" + msisdn;
			data += "&dca=16bit";
			data += "&want_report=1";

			return data;
		}

		static string eight_bit_message( string username, string password, string msisdn, string message ) {
			
			string udh = "0605040B8423F0";

			string wsp_header = "DC0601AE";
			string wap_push_message = udh + wsp_header + message;

			string data = "";
			data += "username="  + HttpUtility.UrlEncode(username, System.Text.Encoding.GetEncoding("ISO-8859-1"));
			data += "&password=" + HttpUtility.UrlEncode(password,  System.Text.Encoding.GetEncoding("ISO-8859-1"));
			data += "&message="  + wap_push_message;
			data += "&msisdn="   + msisdn;
			data += "&dca=8bit";
			data += "&want_report=1";

			return data;
		}
		
		static string get_headers( string msg_type ) {
			string headers = "";
			if( msg_type == "wap_push" ) {
				string udh = "0605040B8423F0";
				string wsp = "DC0601AE";

				headers += udh + wsp;
			}
			else if( msg_type == "vCard" || msg_type == "vCalendar" ) {
				headers += "06050423F40000";
			}
			return headers;
		}
		static string xml_to_string( string msg_body ) {
			//TODO
			/*
			* Code to convert 'msg_body' will go in here.
			*/

			return "conversion";
		}
		static string stringToHex(string s){
			string hex = "";
			foreach (char c in s)
			{
				int tmp = c;
				hex += String.Format("{0:x4}", (uint)System.Convert.ToUInt32(tmp.ToString()));
			}
			return hex;
        }

      #endregion
    }
}
