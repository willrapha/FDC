﻿namespace FDC.Caixa.Tests.Base
{
    public abstract class BuilderBase
    {
        protected void AtribuirId(int id, object entidade)
        {
            if (!TemId(id)) return;

            Atribuir(id, "Id", entidade);
        }

        private bool TemId(long id)
        {
            return id > 0;
        }

        protected void AtribuirId(long id, object entidade)
        {
            if (!TemId(id)) return;

            Atribuir(id, "Id", entidade);
        }

        protected void Atribuir(object valor, string propriedade, object entidade)
        {
            var propertyInfo = entidade.GetType().GetProperty(propriedade);
            propertyInfo.SetValue(entidade, Convert.ChangeType(valor, propertyInfo.PropertyType), null);
        }
    }
}
