using Api.Common.DataTable;
using Api.Contract.DTO;
using Api.Contract.Model;
using AutoMapper; // ���Ӵ�����
using System.Reflection;
using System.Linq;

namespace Api.Web
{
    // �̳� AutoMapper.Profile ��ʹ�� CreateMap ����
    public class LeaveSystemWebAutoMapperProfile : Profile
    {
        public LeaveSystemWebAutoMapperProfile() : base()
        {
            // �Զ�ɨ�� Api.Contract.Model �����ռ��µ������࣬
            // Ϊÿ�����ڶ�Ӧ DTO �� model �Զ���������ӳ�䣺Model->DTO��DTO->Model��DataTableWrapper<Model>->DataTableWrapper<DTO>
            var modelNamespace = "Api.Contract.Model";
            var dtoNamespace = "Api.Contract.DTO";

            // Api.Contract assembly ���� model �� dto ����
            var contractAssembly = typeof(News).Assembly;

            var dtWrapperOpen = typeof(DataTableWrapper<>);

            var modelTypes = contractAssembly.GetTypes()
                .Where(t => t.IsClass && t.Namespace == modelNamespace && !t.IsAbstract)
                .ToList();

            foreach (var modelType in modelTypes)
            {
                var dtoType = contractAssembly.GetType(dtoNamespace + "." + modelType.Name + "DTO");
                if (dtoType == null)
                    continue; // û�ж�Ӧ DTO������

                // ���� Model -> DTO �� DTO -> Model ӳ��
                CreateMap(modelType, dtoType);
                CreateMap(dtoType, modelType);

                // ���� DataTableWrapper ӳ��
                var modelDt = dtWrapperOpen.MakeGenericType(modelType);
                var dtoDt = dtWrapperOpen.MakeGenericType(dtoType);
                CreateMap(modelDt, dtoDt);
            }
            CreateMapByManually();
        }

        private void CreateMapByManually()
        {
            // �����Ҫ�ֶ�����ӳ�䣬��������������
            //CreateMap<Block, BlockDTO>();
            //CreateMap<BlockDTO, Block>();
            //CreateMap<DataTableWrapper<Block>, DataTableWrapper<BlockDTO>>();
            // ����Ϊ���� Model/DTO �����ֶ�ӳ��...
        }
    }
}
