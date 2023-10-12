using TimeDateCalculator.MessageThings;
using System.Threading.Tasks;

namespace TimeDateCalculator.FileHandlers
{
	public interface IHandleFiles
	{
		Task SelectFilesToReadFrom(string[] filetypes);
		Task SelectFilesToSaveTo(string SuggestedNameOfFileToSaveTo, string[] filetypes, string mesgKey);
		Task<bool> PathExists(string PathName);
		Task<bool> FileExists(string FilePathAndName);
		Task<string> ReadFromTextFile(System.IO.Stream TheTextFileStream);
		Task<string> ReadFromTextFile(string FilePathAndName);
		Task<bool> SaveToTextFile(System.IO.Stream TheTextFileStream, string TheText);
		Task<bool> SaveBytesToFile(string thePathAndFile, byte[] ByteBuffer);
		string GetCurrentDirectory();
		Task<byte[]> ReadAllBytesFromFile(System.IO.Stream TheByteFileStream);
	}
}
