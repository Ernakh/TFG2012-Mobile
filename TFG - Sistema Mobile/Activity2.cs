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
using Android.Database.Sqlite;
using Android.Database;

namespace TFG___Sistema_Mobile
{
    [Activity(Label = "Categorias")]
    public class Activity2 : Activity
    {
        private SQLiteDatabase database;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Categorias);

            var checks = FindViewById<LinearLayout>(Resource.Id.Categorias);

            database = OpenOrCreateDatabase("tfg.db", FileCreationMode.Private, null);
        
            ICursor cursor = database.RawQuery("select * from categorias", null);
            
            if (cursor.MoveToFirst())
            {
                do
                {
                    var check = new CheckBox(this);
                    check.Text = cursor.GetString(1).ToString();
                    check.Id = int.Parse(cursor.GetInt(0).ToString());
                    check.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

                    check.Click += new EventHandler(check_Click);

                    checks.AddView(check);

                } while (cursor.MoveToNext());

            }

            database.Close();


        }

        void check_Click(object sender, EventArgs e)
        {
            CheckBox check = (CheckBox)sender;

            if (check.Checked)
            {
                ContentValues values = new ContentValues();
                values.Put("id", check.Id);
                values.Put("descricao", check.Text);
                values.Put("ativo", true);

                if (database.Update("categorias", values, "id = " + check.Id, null) > 0)
                {
					Toast.MakeText(this, "Categoria " + check.Text + " ativada! ", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Falha ao alterar!", ToastLength.Long).Show();
                }
            }
            else
            {
                ContentValues values = new ContentValues();
                values.Put("id", check.Id);
                values.Put("descricao", check.Text);
                values.Put("ativo", false);

                if (database.Update("categorias", values, "id = " + check.Id, null) > 0)
                {
					Toast.MakeText(this, "Categoria " + check.Text + " desativada! ", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Falha ao alterar!", ToastLength.Long).Show();
                }
            }
        }

        /*void btn_Click(object sender, EventArgs e)
        {
            var checks = FindViewById<LinearLayout>(Resource.Id.Categorias);

            Android.Views.View[] x = (Android.Views.View[])checks.FindViewWithTag(0);

            foreach (Android.Views.View item in x)
            {
                
            }

            //CheckBox x = (CheckBox)checks.FindViewWithTag(0);

            Toast.MakeText(this, "Salvo com Sucesso!", ToastLength.Long).Show();
        }*/
    }
}