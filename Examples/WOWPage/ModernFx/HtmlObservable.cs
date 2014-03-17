using System;
using System.Windows.Browser;
using Microsoft.LiveLabs.Html;
using Microsoft.LiveLabs.JavaScript.IL2JS;
using System.Concurrency;
using System.Disposables;
using System.Linq;

namespace WOWPage.ModernFx
{
    public class HtmlObservable : IObservable<HtmlEvent>
    {
        readonly public Action<HtmlEventHandler> addHandler;
        readonly public Action<HtmlEventHandler> removeHandler;

        public HtmlObservable(Action<HtmlEventHandler> addHandler, Action<HtmlEventHandler> removeHandler)
        {
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
        }

        public IDisposable Subscribe(IObserver<HtmlEvent> observer)
        {
            addHandler(observer.OnNext);
            return Disposable.Create(() => removeHandler(observer.OnNext));
        }
    }
}
