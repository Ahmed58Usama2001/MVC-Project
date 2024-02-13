namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            //1. Get Located Folder Path
            //string folderPath = "F:\\Self Study\\.Net Track Self\\MVC Route\\DEMO Solution\\Demo.PL\\wwwroot\\Files\\" + folderName;
            //string folderPath = Directory.GetCurrentDirectory() + @"\wwwroot\Files\" + folderName;
            string folderPath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files" ,folderName);

            //2. Get File name and make it UNIQUE
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            //3. Get file path
            string filePath = Path.Combine(folderPath,fileName);

            //4. Save File as streams [Data Per Time]
          using var fileStream=new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DeleteFile(string fileName,string folderName)
        {
            string filePath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);

            if(File.Exists(filePath))
                File.Delete(filePath);
            
        }
    }
}
