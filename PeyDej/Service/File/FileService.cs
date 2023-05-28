using Microsoft.Build.Exceptions;

using PeyDej.Data;
using PeyDej.Enums;
using PeyDej.Models.File;

namespace PeyDej.Service.File;

public class FileService : IFileInterface
{
    public FileService(PeyDejContext context)
    {
        Context = context;
    }

    private PeyDejContext Context { get; }
    Guid IFileInterface.AddFile(UploadFor catalog, IFormFile file)
    {
        lock (Thread.CurrentThread)
        {
            var locationFile = GetLocation(catalog);
            if (!Directory.Exists(locationFile)) Directory.CreateDirectory(locationFile);
            var fileInformation = new FileInformation(file, locationFile);
            var destinationFile = Path.Combine(locationFile, fileInformation.Id.ToString());
            SaveFile(file, destinationFile);
            Context.FileInformations.Add(fileInformation);
            Context.SaveChanges();
            return fileInformation.Id;
        }
    }
    Guid IFileInterface.EditFile(UploadFor catalog, IFormFile file, Guid idOldFile)
    {
        //Delete Old File
        if (idOldFile != Guid.Empty)
        {
            var v = Context.FileInformations.FirstOrDefault(I => I.Id == idOldFile);
            if (v is not null)
            {
                var path = Path.Combine(v.FileLocation, v.Id.ToString());//Delete Old File
                if (Directory.Exists(path)) System.IO.File.Delete(path);//Delete Old File
                Context.FileInformations.Remove(v);//Delete Old FileInformation For Database
                Context.SaveChanges();
            }
        }

        ///add New File
        string locationFile = GetLocation(catalog);//orginal File

        if (!Directory.Exists(locationFile)) Directory.CreateDirectory(locationFile);//orginal File

        var fileInformation = new FileInformation(file, locationFile);//orginal File
        var fileId = fileInformation.Id.ToString();//orginal File
        var destinationFile = Path.Combine(locationFile, fileInformation.Id.ToString());
        SaveFile(file, destinationFile);//orginal File

        Context.FileInformations.Add(fileInformation); // Save FileInformation
        Context.SaveChanges();//Save Changes DbContext

        return fileInformation.Id;
    }

    private static void SaveFile(IFormFile file, string destination)
    {
        using FileStream stream = new(destination, FileMode.Create);
        try
        {
            file.CopyTo(stream);
        }
        catch (Exception e)
        {
            throw new InvalidProjectFileException(e.Message);
        }
        finally
        {
            stream.Close();
            stream.Dispose();
        }
    }
    private static string GetLocation(UploadFor catalog) => Path.Combine("C:", "PeyDejFileManager", catalog.ToString(), DateTime.Now.ToString("yy-MM-dd"));
}