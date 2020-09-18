using System;
namespace KarteFormsSample
{
    public interface IKarte
    {
        void Setup(string appkey);
        void View(string title);
    }
}
