using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Zeze.Core.Models;
using Zeze.Core.Models.Vows;
using Zeze.Domin.Models.Vows;
using Zeze.IServices;

namespace Zeze.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VowController : ControllerBase
    {
        private readonly IVowService _vowService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public VowController(IVowService vowService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _vowService = vowService;
            _mapper = mapper;
            _configuration = configuration;
        }


        /// <summary>
        /// 分页获取许愿列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/get/vowPageList")]
        public MessageModel<PageModel<Vow>> GetVowPageList([FromQuery]BasePageModel model)
        {
            var data = new MessageModel<PageModel<Vow>>();
            //var list = this._vowService.GetVowList(model);
            //data.success = true;
            //data.msg = "数据获取成功";
            //data.response.data = list.Item1;
            //data.response.TotalCount = list.Item2;
            return data;
        }

        /// <summary>
        /// 获取许愿列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/get/vowAllList")]
        //[Authorize(Roles = "Admin")]
        public async Task<MessageModel<List<Vow>>> GetVowAllList()
        {
            var data = new MessageModel<List<Vow>>();
            var list = await _vowService.GetVowList();
            data.success = true;
            data.msg = "数据获取成功";
            data.response = list;
            return data;
        }

        //// GET: api/Vow/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        /// <summary>
        /// 许愿
        /// </summary>
        /// <param></param>
        [HttpPost]
        [Route("/post/vow")]
        public async Task<MessageModel<string>> Post([FromQuery]VowCreateModel model)
        {
            var data = new MessageModel<string>();
            var vow = _mapper.Map<Vow>(model);
            data.success = await _vowService.AddVow(vow);
            if (data.success)
            {
                data.msg = "许愿成功";
            }
            return data;
        }

        //// PUT: api/Vow/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        /// <summary>
        /// 取消许愿
        /// </summary>
        [HttpDelete]
        [Route("/delete/vow/{id}")]
        public async Task<MessageModel<string>> Delete(Guid id)
        {
            var data = new MessageModel<string>();
            data.success = await _vowService.DeleteVow(id);
            if (data.success)
            {
                data.msg = "删除成功";
            }
            return data;
        }
    }
}
