namespace Lenceas.Core.Model
{
    public static class MenuExt
    {
        #region Entity->MenuTreeWebModel
        public static MenuTreeWebModel AsTreeWebModel(this Menu entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Menu[] { entity }.AsTreeWebModel().FirstOrDefault();
        }

        public static IEnumerable<MenuTreeWebModel> AsTreeWebModel(this IEnumerable<Menu> entities)
        {
            IEnumerable<MenuTreeWebModel> result = new MenuTreeWebModel[] { };
            if (entities != null && entities.Any())
            {
                result = (from p in entities
                          select new MenuTreeWebModel()
                          {

                          }).ToArray();
            }
            return result;
        }
        #endregion
    }
}