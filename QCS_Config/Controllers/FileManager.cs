using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QCS_Config.Controllers;

[Route("images/")]
[ApiController]

public class FileManager : ControllerBase
{

    private  int getClinicId()
    {
        return Convert.ToInt32(User.Claims.First(a => a.Type.ToLower().Contains("role")).Value);
    }
    
        
    
    [HttpGet("{type}/{id}")]
    public async Task<ActionResult<List<string>>> GalleryList(string type,int id)
    {
        var clinicId = getClinicId();
        var baseAddress = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var path =baseAddress + $"\\images\\{clinicId}";
        if(type=="IntroGallery") path+= "\\Gallery";
        if(type=="ServiceGallery") path+= $"\\Services\\{id}\\Gallery";
        if(type=="DrGallery") path+= $"\\Doctors\\Gallery\\{id}";
        if(type=="GraphyGallery") path+= $"\\Graphy\\{id}";
        if(type=="BillGallery") path+= $"\\Bill\\{id}";
        if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
        var files=System.IO.Directory.GetFiles(path).ToList();
        List<string> res = new List<string>();
        foreach(string file in files)
            res.Add( Path.GetFileName(file));
        return res;

    }
    
    
    [HttpGet("{clinicId}/{type}/{id}/{fileName}")]
    public async Task<ActionResult> DownloadFile(int clinicId,string type,int id,string fileName)
    {

        var baseAddress = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var fileAddress =baseAddress + $"\\images\\{clinicId}";
        if(type=="Intro") fileAddress+= $"";
        if(type=="IntroGallery") fileAddress+= $"\\Gallery";
        if(type=="Service") fileAddress+= $"\\Services\\{id}";
        if(type=="ServiceGallery") fileAddress+= $"\\Services\\{id}\\Gallery";
        if(type=="Dr") fileAddress+= $"\\Doctors\\{id}";
        if(type=="DrGallery") fileAddress+= $"\\Doctors\\Gallery\\{id}";
        if(type=="GraphyGallery") fileAddress+= $"\\Graphy\\{id}";
        if(type=="BillGallery") fileAddress+= $"\\Bill\\{id}";
        if (!System.IO.Directory.Exists(fileAddress)) System.IO.Directory.CreateDirectory(fileAddress);
        fileAddress += $"\\{fileName}";
        if (!System.IO.File.Exists(fileAddress)) return NotFound(new { Error = "File Not Found" });
        byte[] bytes = System.IO.File.ReadAllBytes(fileAddress);
        var result = new FileContentResult(bytes, "image/jpeg");
        return result;
    }
    
    
    [HttpPost("{type}/{id}/{fileName}")]
    public async Task<ActionResult> WriteFile(string type,int id,string fileName, IFormFile file)
    {
        var clinicId = getClinicId();
        try
        {
        if (file==null) return NotFound();
        
        var stream =file.OpenReadStream();
        var baseAddress = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var fileAddress =baseAddress + $"\\images\\{clinicId}";
        if(type=="Intro") fileAddress+= $"";
        if(type=="IntroGallery") fileAddress+= $"\\Gallery";
        if(type=="Service") fileAddress+= $"\\Services\\{id}";
        if(type=="ServiceGallery") fileAddress+= $"\\Services\\{id}\\Gallery";
        if(type=="Dr") fileAddress+= $"\\Doctors\\{id}";
        if(type=="DrGallery") fileAddress+= $"\\Doctors\\Gallery\\{id}";
        if(type=="GraphyGallery") fileAddress+= $"\\Graphy\\{id}";
        if(type=="BillGallery") fileAddress+= $"\\Bill\\{id}";
        if (!System.IO.Directory.Exists(fileAddress)) System.IO.Directory.CreateDirectory(fileAddress);
        fileAddress += $"\\{fileName}";
        using (FileStream fs = System.IO.File.Create(fileAddress))
        {
            file.CopyTo(fs);
            fs.Flush();
        }
        stream.Close();
        }
        catch (Exception e)
        {
            return NoContent();
        }
        return Ok();

    }

    [HttpPost("{type}/{id}/")]
    public async Task<ActionResult> manyWriteFile(string type,int id )
    {
        var clinicId = getClinicId();
        var files = Request.Form.Files;
        if (files == null) return NotFound();
        int index = 0;

        var baseAddress = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var path =baseAddress + $"\\images\\{clinicId}";
        if(type=="IntroGallery") path+= "\\Gallery";
        if(type=="ServiceGallery") path+= $"\\Services\\{id}\\Gallery";
        if(type=="DrGallery") path+= $"\\Doctors\\Gallery\\{id}";
        if(type=="GraphyGallery") path+= $"\\Graphy\\{id}";
        if(type=="BillGallery") path+= $"\\Bill\\{id}";
        if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
        foreach (var formFile in files)
        {
                
            var stream =formFile.OpenReadStream();
           
            var fileName =  $"{path}\\{DateTime.Now.ToString("yyyyMMddHHmmssfff")+index}";
            
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                formFile.CopyTo(fs);
                fs.Flush();
            }
            stream.Close();
            index++;
        }
        return Ok();

    }
    
    
    [HttpDelete("{type}/{id}/{fileName}")]
    public async Task<ActionResult> RemoveGallery(string type,int id,string fileName)
    {
        var clinicId = getClinicId();
        var baseAddress = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var fileAddress =baseAddress + $"\\images\\{clinicId}";
        if(type=="Intro") fileAddress+= $"";
        if(type=="IntroGallery") fileAddress+= $"\\Gallery";
        if(type=="Service") fileAddress+= $"\\Services\\{id}";
        if(type=="ServiceGallery") fileAddress+= $"\\Services\\{id}\\Gallery";
        if(type=="Dr") fileAddress+= $"\\Doctors\\{id}";
        if(type=="DrGallery") fileAddress+= $"\\Doctors\\Gallery\\{id}";
        if(type=="GraphyGallery") fileAddress+= $"\\Graphy\\{id}";
        if(type=="BillGallery") fileAddress+= $"\\Bill\\{id}";
        if (!System.IO.Directory.Exists(fileAddress)) System.IO.Directory.CreateDirectory(fileAddress);
        fileAddress += $"\\{fileName}";
        if (!System.IO.File.Exists(fileAddress)) return NotFound(new { Error = "File Not Found" });
        System.IO.File.Delete(fileAddress);

        return Ok();

    }
    
    
        
 


    // [HttpGet("Gallery/{type}/{fileName}")]
    // public async Task<ActionResult> GetGalleryFile(string type,string fileName)
    // {
    //     var clinicId = getClinicId();
    //     var baseAddress = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    //     var fileAddress = baseAddress + $"\\images\\{clinicId}\\Gallery\\{fileName}";
    //     if (!System.IO.File.Exists(fileAddress)) Directory.CreateDirectory(baseAddress + $"\\images\\{clinicId}\\Gallery\\");;
    //     byte[] bytes = System.IO.File.ReadAllBytes(fileAddress);
    //     var result = new FileContentResult(bytes, "image/jpeg");
    //     return result;
    //
    // }


   
}