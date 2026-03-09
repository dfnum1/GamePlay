//auto generated
namespace Framework.AT.Runtime
{
	[Framework.Base.EditorSetupInit]
	internal class ATRegisterHandler
	{
		//-----------------------------------------------------
		public static void Init()
		{
			Register(984305729, typeof(TopGame.Data.CsvData_Models),TopGame.Data.TopGame_Data_CsvData_Models.DoAction,322352634/*Framework.Data.Data_Base*/);
			Register(1963712363, typeof(TopGame.Data.CsvData_Models.ModelsData),TopGame.Data.TopGame_Data_CsvData_Models+ModelsData.DoAction,704662154/*Framework.Data.ABaseData*/);
		}
		//-----------------------------------------------------
		public static void Register(int typeId, System.Type type, ATCallHandler.OnActionDelegate onFunction, int parentTypeId =0)
		{
			ATRtti.Register(typeId,type,parentTypeId);
			ATCallHandler.RegisterHandler(typeId,onFunction);
		}
	}
}
