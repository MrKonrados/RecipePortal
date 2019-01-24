using AutoMapper;
using AutoMapper.EntityFramework;
using AutoMapper.EquivalencyExpression;
using RecipePortal.Dtos;
using RecipePortal.Models;

namespace RecipePortal.App_Start
{
    public static class AutomapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.SetGeneratePropertyMaps<GenerateEntityFrameworkPrimaryKeyPropertyMaps<ApplicationDbContext>>();

                cfg.CreateMap<RecipeDto, Recipe>(MemberList.Source)
                    .EqualityComparison(((dto, recipe) => dto.Id == recipe.Id));

                cfg.CreateMap<IngredientDto, Ingredient>(MemberList.Source)
                    .EqualityComparison(((dto, ingredient) => dto.Id == ingredient.Id));

            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}