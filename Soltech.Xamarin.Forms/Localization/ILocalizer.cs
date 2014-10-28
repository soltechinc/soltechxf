using System;

namespace SolTech.Forms
{
    public interface ILocalizer
    {
        String GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs);
    }
}
