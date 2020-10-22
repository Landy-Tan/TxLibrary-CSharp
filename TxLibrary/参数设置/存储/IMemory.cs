/**********************************************************************
 * File name:		IMemory.cs
 * Author:			landy
 * Create date:		2020年10月22日
 * Description:		存储接口
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年10月22日   landy       创建Save/Load接口
 **********************************************************************/

namespace TxLibrary.Config.Memory
{
    /// <summary>
    /// 参数存储接口
    /// </summary>
    public interface IMemory<T>
    {
        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="file"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Save(string file, T obj);

        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        T Load(string file);
    }
}
