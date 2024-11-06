﻿using MaterialAdvisor.Storage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaterialAdvisor.API.Controllers;

[Authorize]
public class FileController(IStorageService _storageService) : BaseApiController
{ 
    [HttpGet]
    public async Task<IActionResult> Download(string file)
    {
        var fileToDownload = await _storageService.GetFile(file);
        return File(fileToDownload.Data, System.Net.Mime.MediaTypeNames.Application.Octet, fileToDownload.OriginalName);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var savedFile = await _storageService.SaveFile(file);
        return Ok(savedFile);
    }
}
