using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebHelper
{
    public class ResultTools
    {
        /// <summary>
        /// 封装jsonResult
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static JsonResult ToJsonResult(object data)
        {
            var obj = new JsonResult();
            obj.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            obj.Data = data;
            return obj;
        }
        /// <summary>
        /// 返回 ResultEntity false数据
        /// </summary>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static JsonResult ToErrResult(string errmsg)
        {
            ResultEntity result = new ResultEntity();
            result.Result = false;
            result.Message = errmsg;
            return ToJsonResult(result);
        }
        /// <summary>
        /// 返回 ResultEntity true数据
        /// </summary>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static JsonResult ToSuccessResult(string errmsg)
        {
            ResultEntity result = new ResultEntity();
            result.Result = true;
            result.Message = errmsg;
            return ToJsonResult(result);
        }
        /// <summary>
        /// 返回单个实体对象的ResultEntity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ent"></param>
        /// <returns></returns>
        public static JsonResult ToEntityResult<T>(T ent)
        {
            ResultEntity<T> result = new ResultEntity<T>();
            result.Result = true;
            result.DataInfo = ent;
            return ToJsonResult(result);
        }
        /// <summary>
        /// 返回实体列表的ResultEntity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static JsonResult ToListResult<T>(List<T> list)
        {
            ResultEntity<T> result = new ResultEntity<T>();
            result.Result = true;
            result.DataList = list;
            return ToJsonResult(result);
        }
    }
}
