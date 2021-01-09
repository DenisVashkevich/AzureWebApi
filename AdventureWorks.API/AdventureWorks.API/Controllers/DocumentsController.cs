﻿
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AdventureWorks.DocStorage.Interfaces;
using AdventureWorks.DocStorage.Models;
using System;

namespace AdventureWorks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentStorageService _documentStorageService;


        public DocumentsController(IDocumentStorageService documentStorageService)
        {
            _documentStorageService = documentStorageService;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadDocumentAsync ([FromForm(Name = "file")]IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                return BadRequest(file);
            }

            Uri result;
            using (var stream = file.OpenReadStream())
            {
                result = await _documentStorageService.AddDocumentAsync(new WordDocumentModel
                {
                    FileName = file.FileName,
                    FileContent = stream,
                    ContentType = file.ContentType
                });
            }

            return Ok(new {url =  result.AbsoluteUri});
        }
    }
}
