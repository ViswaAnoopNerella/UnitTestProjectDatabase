using DataTracking.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;


namespace UnitTestProjectDataBase
{
	//[TestClass]
	public class UnitTestTraceAbility_backup
	{
		readonly DataTracking.DAL.ITraceAbility TraceabilityOp;


		public UnitTestTraceAbility_backup()
		{

			ConnectionStringInfo connectionStringInfo = new ConnectionStringInfo();
			//connectionStringInfo = new ConnectionStringInfo();
			connectionStringInfo.UserID = "RemoteUser";//machineMES
			connectionStringInfo.Password = "RemotePassword";//machineMES
			connectionStringInfo.TraceabilityServer = @"DCZZ189776_OLD\sqlexpress";// @"satddb01.CORP.SENSATA.COM";
			connectionStringInfo.TraceabilityDBName = "MESDataBaseTesting";//Anoop modified
			connectionStringInfo.ApplicationName = "C0001";//"C0001"

			//Test without DB
			//TraceabilityOp = new DataTracking.DAL.TestTraceAbility(connectionStringInfo, false);

			//Production
			TraceabilityOp = new DataTracking.DAL.TraceAbility(connectionStringInfo, false);
		}

		[TestMethod]
		public void TestMethodStartLot()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "23330166A";
			lotInfo.MachineID = "C0001";
			lotInfo.Operator = "A1020444";//A1020353//A1020444
			lotInfo.Order = "57805702";
			lotInfo.Product = "80600600";
			lotInfo.Quantity = 200;
			lotInfo.ShiftLeader = "H";

			var result = TraceabilityOp.StartLot(lotInfo);
			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}

			}
			else
				foreach (var properties in lotInfo.GetType().GetProperties())
				{
					object tmp = properties.GetValue(lotInfo);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}

			//on success read lotInfo.LotIdent and lotInfo.Quantity

			Assert.AreEqual(result.Success, true);
			//Assert.AreNotEqual(lotInfo.LotIdent , 0);
		}

		[TestMethod]
		public void TestMethodStartLotFail()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M012";
			lotInfo.Operator = "6062";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			var result = new DataTracking.Models.OperationResult();

			result = TraceabilityOp.StartLot(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}

			}
			else
				foreach (var properties in lotInfo.GetType().GetProperties())
				{
					object tmp = properties.GetValue(lotInfo);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}

			Assert.AreEqual<bool>(result.Success, false);
			Assert.AreEqual<bool>(result.MessagesList.Count > 0, true);
		}

		[TestMethod]
		public void TestMethodEndLotOK()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "23330166A";
			lotInfo.MachineID = "C0001";
			lotInfo.Operator = "A116062";
			lotInfo.Order = "57805702";
			lotInfo.Product = "80600600";
			lotInfo.Quantity = 200;
			lotInfo.ShiftLeader = "H";

			lotInfo.LotIdent = 1;
			lotInfo.PiecesOK = 25;


			var result = TraceabilityOp.EndLotOK(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}

			}
			else
				foreach (var properties in lotInfo.GetType().GetProperties())
				{
					object tmp = properties.GetValue(lotInfo);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}

			Assert.AreEqual(result.Success, true);
			//Assert.AreNotEqual(lotInfo.LotIdent, 0);
		}

		[TestMethod]
		public void TestMethodAddScrap()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M7020";
			lotInfo.Operator = "6062";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			lotInfo.LotIdent = 39001757;
			lotInfo.PiecesOK = 99;

			var scrapInfo = new DataTracking.Models.ScrapInfo();
			scrapInfo.ScrapCode = 710;
			scrapInfo.ScrapCount = 1;

			var result = TraceabilityOp.AddScrap(lotInfo, scrapInfo);

			foreach (var properties in lotInfo.GetType().GetProperties())
			{
				object tmp = properties.GetValue(lotInfo);
				if (tmp != null)
					Console.WriteLine(properties.Name + " " + tmp.ToString());
			}

			Assert.AreEqual(result.Success, true);
			Assert.AreNotEqual(lotInfo.LotIdent, 0);
		}

		[TestMethod]
		public void TestMethodComponentsValidation()
		{
			var componentInfo = new DataTracking.Models.ComponentInfo();

			componentInfo.Ident = 1;//1//39001757
			componentInfo.Item = "80600612";//80600612//52000421
			componentInfo.Seria = "23330166A"; //23330166A//BG010865672
			componentInfo.Quan = 1;

			var result = TraceabilityOp.ComponentsValidation(componentInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}

			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMachineDefects()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "23330166A";//16442400P
			lotInfo.MachineID = "C0001";
			lotInfo.Operator = "A1020444";//6062
			lotInfo.Order = "57805702";//40978595
			lotInfo.Product = "80600600";//52001960
			lotInfo.Quantity = 200;//100
			lotInfo.ShiftLeader = "H";

			var result = TraceabilityOp.GetMachineDefects(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}

			}

			if (result.Success)
			{
				foreach (var item in result.ResultObject)
				{
					foreach (var properties in item.GetType().GetProperties())
					{
						object tmp = properties.GetValue(item);
						if (tmp != null)
							Console.WriteLine(properties.Name + " " + tmp.ToString());
					}
				}

			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodEndLotComponent()
		{

			var componentInfo = new DataTracking.Models.ComponentInfo();

			componentInfo.Ident = 1;//21301605
			componentInfo.Item = "80600612";//52000456
			componentInfo.Seria = "23330166A";//16140049M
			componentInfo.Quan = 1;//5000

			int usedComponents = 1;

			var result = TraceabilityOp.EndLotComponent(componentInfo, usedComponents, false);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}

			}

			Assert.AreEqual(result.Success, false);
		}

		[TestMethod]
		public void TestMethodAddScrapComponentByReason()
		{

			var componentInfo = new DataTracking.Models.ComponentInfo();

			componentInfo.Ident = 1;//21301605
			componentInfo.Item = "80600612";//52000456
			componentInfo.Seria = "23330166A";//16140049M
			componentInfo.Quan = 1;//5000

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.LotIdent = 1;//21301605
			lotInfo.Lot = "23330166A";//17051462P
			lotInfo.MachineID = "C0001";//M0105
			lotInfo.Operator = "A1020444";//6062
			lotInfo.Order = "57805702";//41416051
			lotInfo.Product = "80600600";//52001278
			lotInfo.Quantity = 200;//100
			lotInfo.ShiftLeader = "H";//H

			var scrapInfo = new DataTracking.Models.ScrapInfo();
			scrapInfo.ScrapCode = 5;
			scrapInfo.ScrapCodeDescr = "Broken";
			scrapInfo.ScrapCount = 1;

			var result = TraceabilityOp.AddScrapComponentByReason(lotInfo, componentInfo, scrapInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}

			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetInstructionLink()
		{

			var result = TraceabilityOp.GetInstructionLink("C0001", "QMS01242637");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}

			}
			else
			{
				Console.WriteLine("URL:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetDrawingLink()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M7020";
			lotInfo.Operator = "6062";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			var result = TraceabilityOp.GetDrawingLink(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				Console.WriteLine("URL:" + result.ResultObject);
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetProdLotComponents()
		{
			var lotInfo = new DataTracking.Models.LotInfo();
			//lotInfo.Lot = "23330166A";
			//lotInfo.Product = "80600600";
			//lotInfo.Order = "57805702";
			//lotInfo.Quantity = 200;
			//lotInfo.Operator = "A1020353";
			//lotInfo.ShiftLeader = "H";
			//lotInfo.MachineID = "C0001";
			lotInfo.LotIdent = 1;//39001757
								 //lotInfo.PiecesLeft = 200;
								 //lotInfo.PiecesOK = 0;
								 //lotInfo.PiecesNOK = 0;

			var result = TraceabilityOp.GetProdLotComponents(lotInfo);

			if (result.Success)
			{
				foreach (var item in result.ResultObject)
				{
					foreach (var properties in item.GetType().GetProperties())
					{
						object tmp = properties.GetValue(item);
						if (tmp != null)
							Console.WriteLine(properties.Name + " " + tmp.ToString());
					}
				}

			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetRFIDAccess()
		{

			var result = TraceabilityOp.GetRFIDAccess("0103C5932B");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}

			if (result.Success)
			{

				foreach (var properties in result.ResultObject.GetType().GetProperties())
				{
					object tmp = properties.GetValue(result.ResultObject);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}

			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetRFIDAccessWithLotInfo()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.MachineID = "C0001";

			var result = TraceabilityOp.GetRFIDAccess("004431433136333636340D0A0000000000", lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}

			if (result.Success)
			{

				foreach (var properties in result.ResultObject.GetType().GetProperties())
				{
					object tmp = properties.GetValue(result.ResultObject);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}

			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetOperationLeftQty()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M7020";
			lotInfo.Operator = "6062";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			lotInfo.LotIdent = 39001757;
			lotInfo.PiecesOK = 99;

			var result = TraceabilityOp.GetOperationLeftQty(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}

			if (result.Success)
			{

				Console.WriteLine("Left PCS:" + " " + result.ResultObject);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetLotInfoFromLotBarCode()
		{
			var result = TraceabilityOp.GetLotInfoFromLotBarCode("80600600+57805702+23330166B+200\r");//23330166A
																									  //"80600600+57805702+23330166A+200\r"
																									  //Barcode = "ProductCode+OrderNo+Lot+Quantity\r"

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Product:" + result.ResultObject.Product);
				Console.WriteLine("Order:" + result.ResultObject.Order);
				Console.WriteLine("Lot:" + result.ResultObject.Lot);
				Console.WriteLine("Quantity:" + result.ResultObject.Quantity);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetComponentInfoFromComponentBarCode()
		{
			var result = TraceabilityOp.GetComponentInfoFromComponentBarCode("T23330166A+Q001+P80600615");//T09170398P+Q050+P52000792

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Product:" + result.ResultObject.Item);
				Console.WriteLine("Lot:" + result.ResultObject.Seria);
				Console.WriteLine("Quantity:" + result.ResultObject.Quan);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetComponentInfoFromComponentBarCodeFail()
		{
			var result = TraceabilityOp.GetComponentInfoFromComponentBarCode("T09170398P+Q0T50+P52000792\r");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Product:" + result.ResultObject.Item);
				Console.WriteLine("Lot:" + result.ResultObject.Seria);
				Console.WriteLine("Quantity:" + result.ResultObject.Quan);
			}

			Assert.AreEqual(result.Success, false);
		}

		[TestMethod]
		public void TestMethodGetOperatorInfoFromOperatorBarCode()
		{
			var result = TraceabilityOp.GetOperatorInfoFromOperatorBarCode("2HCA1020444\r");//2HCA1020444

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Operator:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetOperatorInfoFromOperatorBarCodeFail()
		{
			var result = TraceabilityOp.GetOperatorInfoFromOperatorBarCode("2HA1020444\r");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Operator:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, false);
		}

		[TestMethod]
		public void TestMethodFailAddMachineResult()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "C0001";
			lotInfo.Operator = "6062";//0
			lotInfo.Order = "40978595";//0
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";
			lotInfo.LotIdent = 1;//0


			DateTime dateTime = new DateTime();
			dateTime = DateTime.Now;
			var dateTimeString = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff");
			var result = TraceabilityOp.AddMachineResult(lotInfo, "10.1+56+26+60+1+1+2017-01-25T13:15:30+0", "InsulationTest+RoomTempRef+RoomTempSensor+Measurements+Station+Verification+DateTime+Result");//Result is scrapcode, 0 is no scrap

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}

			Assert.AreEqual(result.Success, false);
		}

		[TestMethod]
		public void TestMethodAddLeakMachineResult()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.LotIdent = 1;//23054860

			var result = TraceabilityOp.AddMachineResult(lotInfo, "OK+22+5+NEST1", "InsulationTest+RoomTempRef+RoomTempSensor+Measurements+Station+Verification+DateTime+Result");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMachineSettings()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.LotIdent = 1;//29924435
			lotInfo.MachineID = "C0001";//M5014


			var result = TraceabilityOp.GetMachineSettings(lotInfo, "ProductLength+Tolerance");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("MachineSetting:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMachineSettingWithFields()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.LotIdent = 47533449;
			lotInfo.MachineID = "M7565";


			var result = TraceabilityOp.GetMachineSettingWithFields(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("ProductSetting:" + result.ResultObject.ProductSetting);
				Console.WriteLine("ProductStructure:" + result.ResultObject.ProductStructure);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestFailGetMachineSettings()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.LotIdent = 1;


			var result = TraceabilityOp.GetMachineSettings(lotInfo, "ProductType+LaserProgram+MinRPM+MaxRPM");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("MachineSetting:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, false);
		}

		[TestMethod]
		public void TestMethodFailAddNewOperator()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.LotIdent = 1;
			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "C0001";
			lotInfo.Operator = "a1063343";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			var result = TraceabilityOp.AddNewOperator(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodAddMachineWorkLoadTime()
		{
			DateTime recDateTime = DateTime.Now;
			string machineID = "M7020";
			string stationID = null;
			int globalStatus = 0; int errorState = 0; int typeState = 0; string descriptionState = "Error";

			var result = TraceabilityOp.AddMachineWorkLoadTime(machineID, recDateTime, stationID, globalStatus, errorState, typeState, descriptionState);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodAddMachineJobSetupOvenGlassSealing()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M7020";
			lotInfo.Operator = "6062";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			lotInfo.LotIdent = 21124244;
			lotInfo.PiecesOK = 99;

			decimal aValue = 0.4M;
			decimal bValue = 1.2M;
			decimal cValue = 0;
			decimal dValue = 0;
			decimal eValue = 0;

			string typeOven = "DartsSealing";

			var result = TraceabilityOp.AddMachineJobSetupOvenGlassSealing(lotInfo, aValue, bValue, cValue, dValue, eValue, typeOven);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodFailMachineStop()
		{
			string machineID = "M";

			string stopUser = "abb";

			string stopReason = "misialgment";
			int stopReasonID = 1;
			string stopStation = null; //"Inserter";//null

			var result = TraceabilityOp.MachineStop(machineID, stopUser, stopReason, stopReasonID, stopStation);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, false);
		}

		[TestMethod]
		public void TestMethodMachineStart()
		{
			string machineID = "1";

			string startUser = "abb";

			var result = TraceabilityOp.MachineStart(machineID, startUser);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodFalseMachineStopMaint()
		{
			string machineID = "M5027";

			string stopUserMaint = "Анонимен";

			var result = TraceabilityOp.MachineStopMaint(machineID, stopUserMaint);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, false);
		}

		[TestMethod]
		public void TestMethodDTCTrayGetInfo()
		{
			string trayID = "0";

			Int16 sNumber;
			string trayInfo;

			Int32 lastOperationStatus = 1;//fail code or pass(0) or empty(-2)

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.MachineID = "M7504";

			var result = TraceabilityOp.DTCTrayGetInfo(trayID, lotInfo, out sNumber, out lastOperationStatus, out trayInfo);

			Console.WriteLine("SNumber:" + sNumber.ToString());
			Console.WriteLine("lastOperationStatus:" + lastOperationStatus);
			Console.WriteLine("trayInfo:" + trayInfo);

			if (!result.Success)
			{

				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				foreach (var properties in lotInfo.GetType().GetProperties())
				{
					object tmp = properties.GetValue(lotInfo);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodDTCTrayGetAll()
		{
			//throw new NotImplementedException();

			string trayID = "090003B75D";
			string trayInfo;

			Int16 sNumber;

			Int32 lastOperationStatus = 1;//fail code or pass(0) or empty(-2)

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.MachineID = "M5027";

			var result = TraceabilityOp.DTCTrayGetInfo(trayID, lotInfo, out sNumber, out lastOperationStatus, out trayInfo);

			Console.WriteLine("SNumber:" + sNumber.ToString());
			Console.WriteLine("lastOperationStatus:" + lastOperationStatus);

			if (!result.Success)
			{

				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				foreach (var properties in lotInfo.GetType().GetProperties())
				{
					object tmp = properties.GetValue(lotInfo);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodDTCTrayEmpty()
		{

			string trayID = "090003B75D";

			Int32 lastOperationStatus = 1;//fail code or pass(0) or empty(-2)

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.MachineID = "M5027";

			var result = TraceabilityOp.DTCTrayEmpty(trayID, lotInfo);

			Console.WriteLine("lastOperationStatus:" + lastOperationStatus);

			if (!result.Success)
			{

				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				foreach (var properties in lotInfo.GetType().GetProperties())
				{
					object tmp = properties.GetValue(lotInfo);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}
			}
			Assert.AreEqual(result.Success, true);

		}

		[TestMethod]
		public void TestMethodDTCTrayAddInfo()
		{
			string trayID = "090003B75D";
			string trayInfo = "4.5";

			Int16 sNumber = 1;
			Int32 lastOperationStatus = 1;//fail code or pass(0) or empty(-2)

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M5027";
			lotInfo.Operator = "6062";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			var result = TraceabilityOp.DTCTrayAddInfo(trayID, lotInfo, sNumber, lastOperationStatus, trayInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodCheckContactBlock()
		{
			string rfidContact = "490079F77";

			string cbType = "";
			Int32 cbCycle = 0;

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "18010003A";
			lotInfo.MachineID = "C0119";

			var result = TraceabilityOp.CheckContactBlock(lotInfo, rfidContact, out cbType, out cbCycle);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodCheckContactBlockFail()
		{
			string rfidContact = "0103E8A53";

			string cbType = "";
			Int32 cbCycle = 0;

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "18042239A";
			lotInfo.MachineID = "S0009";

			var result = TraceabilityOp.CheckContactBlock(lotInfo, rfidContact, out cbType, out cbCycle);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, false);
		}

		[TestMethod]
		public void TestMethodAddContactBlockQuantity()
		{
			string rfidContact = "0103E8A534";

			Int32 cbCycle = 1;

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "18042239A";
			lotInfo.MachineID = "S0009";

			var result = TraceabilityOp.AddContactBlockQuantity(lotInfo, rfidContact, cbCycle);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodCheckIncomingTare()
		{
			string tare = "5104";

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "18070833P";
			lotInfo.LotIdent = 25867153;

			var result = TraceabilityOp.CheckIncomingTare(lotInfo, tare);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.ErrorCode, 2);

		}

		[TestMethod]
		public void TestMethodAddOutputTare()
		{
			string tare = "5104";

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "18070833P";
			lotInfo.LotIdent = 2;

			var result = TraceabilityOp.AddOutputTare(lotInfo, tare);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.ErrorCode, 2);

		}

		[TestMethod]
		public void TestMethodCheckNeedTares()
		{

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "18070833P";
			lotInfo.LotIdent = 2;

			var result = TraceabilityOp.CheckNeedTares(lotInfo, out bool isNeedInputTare, out bool isNeedOutputTare);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				Console.WriteLine("InputTare:" + isNeedInputTare + "OutputTare:" + isNeedOutputTare);
			}
			Assert.AreEqual(result.ErrorCode, 0);

		}
		[TestMethod]
		public void TestMethodGetDowntimeLink()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M7020";
			lotInfo.Operator = "6062";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			var result = TraceabilityOp.GetDowntimeLink(lotInfo.MachineID, lotInfo.Operator, "");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				Console.WriteLine("URL:" + result.ResultObject);
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodCheckPrevOperationQty()
		{

			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "18170594A";
			lotInfo.MachineID = "S0108";
			lotInfo.Operator = "6062";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			var result = TraceabilityOp.CheckPrevOperationQty(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				foreach (var properties in lotInfo.GetType().GetProperties())
				{
					//on no pcs on prev operation calculate by start and scrap pcs
					object tmp = properties.GetValue(lotInfo);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodAddDownTimeWindowState()
		{
			DateTime recDateTime = DateTime.Now;
			string machineID = "M7583";
			string stationID = "1";
			int timeDelaySec = 0;
			int globalStatus = 1005; int errorState = 1; int typeState = 10; string descriptionState = "Непланиран ремонт";
			string userCardID = "abb";

			var result = TraceabilityOp.AddDownTimeWindowState(machineID, timeDelaySec, stationID, globalStatus, errorState, typeState, descriptionState, userCardID);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetDownTimeWindowState()
		{
			DateTime recDateTime = DateTime.Now;
			string machineID = "M7583";
			string stationID = "1";
			int globalStatus = 0; int typeState = 0; string descriptionState = "";

			var result = TraceabilityOp.GetDownTimeWindowState(machineID, stationID, out globalStatus, out int errorState, out typeState, out descriptionState);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				Console.WriteLine("globalStatus" + globalStatus.ToString());
				Console.WriteLine("errorState" + errorState.ToString());
				Console.WriteLine("typeState" + typeState.ToString());
				Console.WriteLine("descriptionState" + descriptionState.ToString());
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetDowntimeLinkWithTimeDelay()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M7020";
			lotInfo.Operator = "6062";
			lotInfo.Order = "40978595";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";

			var result = TraceabilityOp.GetDowntimeLink(lotInfo.MachineID, lotInfo.Operator, "", 1);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				Console.WriteLine("URL:" + result.ResultObject);
			}
			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMachineGoldenSampleSettings()
		{
			//OperationResult<bool>
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.LotIdent = 29924435;
			lotInfo.MachineID = "M5014";


			var result = TraceabilityOp.GetMachineGoldenSampleSettings(lotInfo, out int timeLeftSec);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("MachineSetting:" + result.ResultObject);
				Console.WriteLine("TimeLeftSec:" + timeLeftSec.ToString());
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodAddMachineGoldenSampleResults()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M012";
			lotInfo.Operator = "6062";//0
			lotInfo.Order = "40978595";//0
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";
			lotInfo.LotIdent = 1234;//0


			DateTime dateTime = new DateTime();
			dateTime = DateTime.Now;
			var dateTimeString = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff");
			var result = TraceabilityOp.AddMachineGoldenSampleResults(lotInfo, "10.1+56+26+60+1+1+2017-01-25T13:15:30+0", "InsulationTest+RoomTempRef+RoomTempSensor+Measurements+Station+Verification+DateTime+Result");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodEndMachineGoldenSampleLot()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M012";
			lotInfo.Operator = "6062";//0
			lotInfo.Order = "40978595";//0
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";
			lotInfo.LotIdent = 1234;//0

			var result = TraceabilityOp.EndMachineGoldenSampleLot(lotInfo, true);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodAddMeasurementDTCHV_Bending_Results()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M012";
			lotInfo.Operator = "6062";//0
			lotInfo.Order = "40978595";//0
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;
			lotInfo.ShiftLeader = "H";
			lotInfo.LotIdent = 1234;//0

			var result = TraceabilityOp.AddMeasurementDTCHV_Bending_Results(lotInfo, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, DateTime.Now);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMachineSettingsByProductAndMachineID()
		{
			string machineID = "M7706";
			string product = "80600612";//80600612//52002462
			var result = TraceabilityOp.GetMachineSettingsByProductAndMachineID(machineID, product, "ProductLength+Tolerance");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("MachineSetting:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMachineSettingsByProductAndMachineIDWithFields()
		{
			string machineID = "M7565";
			string product = "50630900";
			var result = TraceabilityOp.GetMachineSettingsByProductAndMachineIDWithFields(machineID, product);//"ProductType+ChipNest+CrimpTool+CylinderPressure+ChipDistance+ToleranceMinus+TolerancePlus"

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("ProductSetting:" + result.ResultObject.ProductSetting);
				Console.WriteLine("ProductStructure:" + result.ResultObject.ProductStructure);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodAddMeasurements()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.Lot = "16442400P";
			lotInfo.MachineID = "M012";
			lotInfo.Product = "52001960";
			lotInfo.Quantity = 100;

			var result = TraceabilityOp.AddMeasurements(lotInfo, "10.1+56+26+60+1+1+2017-01-25T13:15:30+0", "InsulationTest+RoomTempRef+RoomTempSensor+Measurements+Station+Verification+DateTime+Result");

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMarkingChipLot()
		{
			var lotInfo = new DataTracking.Models.LotInfo();

			lotInfo.LotIdent = 29924435;
			lotInfo.Lot = "21141416A";
			lotInfo.MachineID = "M5014";


			var result = TraceabilityOp.GetMarkingChipLot(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Chip Lot:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMarkingStartSN()
		{

			string chipLot = "995P"; int quantity = 1;

			var result = TraceabilityOp.GetMarkingStartSN(chipLot, quantity);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Start Number:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodCheckQtyComponents()
		{
			var componentInfo = new DataTracking.Models.ComponentInfo();

			componentInfo.Ident = 39001757;
			componentInfo.Item = "52000421";
			componentInfo.Seria = "BG016571976";
			componentInfo.Quan = 36000;

			var result = TraceabilityOp.CheckQtyComponents(componentInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}

			}

			if (result.Success)
			{
				Console.WriteLine("ActualQuan:" + componentInfo.ActualQuan);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMachineOST_FFTProducedParts()
		{

			var lotInfo = new DataTracking.Models.LotInfo
			{
				LotIdent = 1,
				Lot = "21180867A",
				Product = "8G06325CC0003-00",
				MachineID = "MR003"
			};

			var result = TraceabilityOp.GetMachineOST_FFTProducedParts(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Produced Parts:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetOSTProductbyLotAndSerialNumber()
		{

			var lotInfo = new DataTracking.Models.LotInfo
			{
				LotIdent = 1,
				Lot = "21183514A",
			};

			string serialNumber = "2010063109";
			var result = TraceabilityOp.GetOSTProductbyLotAndSerialNumber(lotInfo, serialNumber);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Product:" + result.ResultObject);
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetMachineOST_FFTDayCountParts()
		{

			var lotInfo = new DataTracking.Models.LotInfo
			{
				LotIdent = 18205,
				Lot = "21184488A",
				Product = "8G62060CC2508-01",
				MachineID = "MR003"
			};


			var result = TraceabilityOp.GetMachineOST_FFTDayCountParts(lotInfo, out DateTime recDate);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				Console.WriteLine("Counter:" + result.ResultObject);
				Console.WriteLine("Date:" + recDate.ToString());
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodGetCurentProdLotByMachineID()
		{

			string machineID = "M0179";
			OperationResult<LotInfo> result = TraceabilityOp.GetCurentProdLotByMachineID(machineID);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			if (result.Success)
			{
				foreach (var properties in result.ResultObject.GetType().GetProperties())
				{
					object tmp = properties.GetValue(result.ResultObject);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}
			}

			Assert.AreEqual(result.Success, true);
		}

		[TestMethod]
		public void TestMethodAddNewOperator()
		{
			string NewOperatorID = "A1063343";
			LotInfo lotInfo = new LotInfo
			{
				LotIdent = 1,
				Operator = NewOperatorID
			};
			var result = TraceabilityOp.AddNewOperator(lotInfo);

			if (!result.Success)
			{
				foreach (var item in result.MessagesList)
				{
					Console.WriteLine("Error Descr:" + item);
					Console.WriteLine("Error Code:" + result.ErrorCode);
				}
			}
			else
			{
				foreach (var properties in lotInfo.GetType().GetProperties())
				{
					object tmp = properties.GetValue(lotInfo);
					if (tmp != null)
						Console.WriteLine(properties.Name + " " + tmp.ToString());
				}
			}
			Assert.AreEqual<bool>(result.Success, false);
			Assert.AreEqual<bool>(result.MessagesList.Count > 0, true);
			//Assert.AreEqual(result.Success, true);

		}

	}
}