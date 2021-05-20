using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TFG___Sistema_Mobile
{
    [Activity(Label = "Produtos")]
    public class Activity3 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Produtos);

            /*List<string> produtos = new List<string>();
            produtos.Add("Feira,brinquedo,12.5");
            produtos.Add("Shopping,livro x,1.5");
            produtos.Add("Mercado,jogo,123.5");*/

			List<string> produtos = (List<string>)Intent.GetStringArrayListExtra("produtos");
            TextView prods = FindViewById<TextView>(Resource.Id.textProd);

			foreach (string item in produtos) 
			{
				string[] x = item.Split (',');

				//TextView tv;
				prods.Text += "Loja:" + x[0] + "\n";
				prods.Text += "Produto:" + x[1] + "\n";
				prods.Text += "Preço: R$" + x[2] + "\n\n\n";

				//prods.AddView(tv);
			}
        }
    }
}