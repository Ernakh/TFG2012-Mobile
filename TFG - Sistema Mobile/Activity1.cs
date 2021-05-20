using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using System.Text;
using Android.Database.Sqlite;
using System.Collections.Generic;

namespace TFG___Sistema_Mobile
{
    [Activity(Label = "TFG___Sistema_Mobile", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        //int count = 1;

        private SQLiteDatabase database;
        private List<string> produtos = new List<string>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            /*Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
                //button.Text = string.Format("{0} clicks!", count++);
            };*/

            Button button2 = FindViewById<Button>(Resource.Id.MyButton2);
            Button button3 = FindViewById<Button>(Resource.Id.MyButton3);
            Button button4 = FindViewById<Button>(Resource.Id.MyButton4);
            Button button5 = FindViewById<Button>(Resource.Id.MyButton5);
            Button button6 = FindViewById<Button>(Resource.Id.MyButton6);

            button2.Click += delegate
            {
                var CatActivity = new Intent(this, typeof(Activity2));

                StartActivity(CatActivity);
            };

            button3.Click += delegate
            {
                database = OpenOrCreateDatabase("tfg.db", FileCreationMode.Private, null);
                database.ExecSQL("drop table categorias");
                database.ExecSQL("drop table lojas");
                database.ExecSQL("drop table coordenadas");
                database.ExecSQL("CREATE TABLE categorias (id integer unique NOT NULL, descricao text, ativo bit);");
				database.ExecSQL("CREATE TABLE coordenadas (loja integer, latitude double, longitude double);");
				database.ExecSQL("CREATE TABLE lojas (id integer, nome varchar(50));");
                database.Close();

				//gravar as lojas e coordenadas vindas do Web Service
				//recebe categorias, lojas e coordenadas do webservice

				GravaCategorias(1, "Esportes");
				GravaCategorias(2, "Livros");

				GravaLoja (1,"Loja 1");
				GravaLoja (2,"Loja 2");

				GravaCoordenadas(1,2222,33333);
				GravaCoordenadas(1,2222,4444444);

				GravaCoordenadas(2,6666666666,99999999);
				GravaCoordenadas(2,7777777777777,111111111);

				//*****************************************************

                Toast.MakeText(this, "Sistema resetado!", ToastLength.Long).Show();
            };

            button4.Click += delegate
            {
                Toast.MakeText(this, "TFG - Fabrício Tonetto Londero(fabriciotonettolondero@gmail.com), orientado por Guilherme Chagas Kurtz (guilhermechagaskurtz@gmail.com).", ToastLength.Long).Show();
            };

            button6.Click += delegate
            {
                testeBd1();
            };

            button5.Click += delegate
            {
                Android.Locations.Location localizacao = pegaPosicao();

                List<string> idLojas = new List<string>();
                

                /*for (int i = 0; i < length; i++)
                {
                    //um for para as empresas/lojas, e dai chamar o VerificaArea passando os arrays de coordenadas de cada loja no metodo
                }*/

                bool naArea = VerificaArea(localizacao.Latitude, localizacao.Longitude);

                if (naArea)
                {
                    //adiciona os ID das lojas onde o celular esta presente
                    idLojas.Add("1");
                    idLojas.Add("3");
                    idLojas.Add("2");
                }
                else
                {
                                     
                }

                if (idLojas.Count == 0)
                {
                    Toast.MakeText(this, "Não encontra-se em nenhuma área!", ToastLength.Long).Show();
                    return;
                }
                else
                {
                    //consome um WebService passando o List idLojas e recebe o List produtos preenchido
                    //produtos = MeuWebService(idLojas);

                    //envia para a Activity dos produtos

					List<string> produtosx = new List<string>();

					produtosx.Add ("1,brinquedo,12.5");
					produtosx.Add ("1,livro x,1.5");
					produtosx.Add ("2,jogo,123.5");

					if(produtosx.Count == 0)
					{
						Toast.MakeText(this, "Nenhum produtos para as restriçoes de lojas e/ou categorias.", ToastLength.Long).Show();
						return;
					}

                    var ProdActivity = new Intent(this, typeof(Activity3));
					ProdActivity.PutStringArrayListExtra("produtos", produtosx);
                    StartActivity(ProdActivity);
				}
			};
        }

		private void GravaCategorias (int id, string descricao)
		{
			database = OpenOrCreateDatabase("tfg.db", FileCreationMode.Private, null);
			
			ContentValues values = new ContentValues();
			values.Put("id", id);
			values.Put("descricao", descricao);
			values.Put("ativo", true);
			
			if (database.Insert("categorias", null, values) > 0)
			{
			}
			else
			{
			}
			
			database.Close();
		}

		private void GravaLoja (int id, string nome)
		{
			database = OpenOrCreateDatabase("tfg.db", FileCreationMode.Private, null);
			
			ContentValues values = new ContentValues();
			values.Put("id", id);
			values.Put("nome", nome);
			
			if (database.Insert("lojas", null, values) > 0)
			{
			}
			else
			{
			}
			
			database.Close();
		}

		private void GravaCoordenadas (int loja, double latitude, double longitude)
		{
			database = OpenOrCreateDatabase("tfg.db", FileCreationMode.Private, null);
			
			ContentValues values = new ContentValues();
			values.Put("loja", loja);
			values.Put("latitude", latitude);
			values.Put("longitude", longitude);
			
			if (database.Insert("coordenadas", null, values) > 0)
			{
			}
			else
			{
			}
			
			database.Close();
		}

        private void AtualizaUltimaPosicao(double lat, double longi)
        {
            database = OpenOrCreateDatabase("tfg.db", FileCreationMode.Private, null);
            database.ExecSQL("drop table ultimaLocalizacao");
            database.ExecSQL("CREATE TABLE ultimaLocalizacao (latitude double, longitude double);");

            ContentValues values = new ContentValues();
            values.Put("latitude", lat);
            values.Put("longitude", longi);

            if (database.Insert("ultimaLocalizacao", null, values) > 0)
            {
            }
            else
            {
            }

            database.Close();
        }

        private Android.Locations.Location pegaPosicao()
        {
            Android.Locations.Location iAmHere;
            LocationManager myLoc = (LocationManager)GetSystemService(Context.LocationService);
            iAmHere = myLoc.GetLastKnownLocation(Android.Content.Context.LocationService);

			return iAmHere;
        }
             
        private void testeBd1 ()
		{
			database = OpenOrCreateDatabase ("tfg.db", FileCreationMode.Private, null);

			//string scripts = @"CREATE TABLE categorias (id integer unique NOT NULL, descricao text, ativo bit);";
			//database.ExecSQL(scripts);
			//string[] splitScripts = scripts.Split(';');

			//for (int i = 0; i < splitScripts.Length; i++)
			//{
			//database.ExecSQL("drop table categorias");
			//database.ExecSQL(scripts);
			//}

			/*ContentValues valuesx = new ContentValues ();
			valuesx.Put ("id", 3);
			valuesx.Put ("descricao", "Esportes");
			valuesx.Put ("ativo", false);
			
			if (database.Insert ("categorias", null, valuesx) > 0) {
			}*/

			ContentValues valuesxx = new ContentValues ();
			valuesxx.Put ("id", 1);
			valuesxx.Put ("descricao", "Livros");
			valuesxx.Put ("ativo", false);
			
			if (database.Insert ("categorias", null, valuesxx) > 0) {
			}

            ContentValues values = new ContentValues();
            values.Put("id", 2);
            values.Put("descricao", "Games");
            values.Put("ativo", false);

            if (database.Insert("categorias", null, values) > 0)
            {
                Toast.MakeText(this, "Contato inserido com sucesso!", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "Falha!", ToastLength.Long).Show();
            }

            /*if (database.Update("categorias", values, "id = 2 ", null) > 0)
            {
                Toast.MakeText(this, "Contato alterado com sucesso!", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "Falha ao alterar!", ToastLength.Long).Show();
            }*/

            //Finish();//fecha a Activity
            database.Close();
        }

        #region GPS
        public void testeGPS()
        {
            LocationManager _locationManager;

            try
            {
                _locationManager = null;
                ILocationListener x = null;

                //if (/*user says to use location services*/)
                // {

                _locationManager = (LocationManager)GetSystemService(Context.LocationService);
                _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 1000, 10, x);
                //}
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, string.Format("{0} -- {1}", ex.Message, ex.StackTrace), ToastLength.Long).Show();
                //_debug.Text = string.Format("{0} -- {1}",ex.Message,ex.StackTrace);
            }
        }

        public void OnLocationChanged(Location location)
        {
            Toast.MakeText(this, string.Format("long: {0}, lat: {1}", location.Longitude, location.Latitude), ToastLength.Long).Show();
            //_debug.Text = string.Format("long: {0}, lat: {1}", location.Longitude, location.Latitude);
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, int status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

        #endregion

        public bool VerificaArea(double lat, double lon)
        {
            double[] xx;
            double[] yy;

            xx = new[] { -33.734837862145795, -33.89172614626097, -33.968069190244236 };
            yy = new[] { 151.04440016174317, 150.86312574768067, 151.1947755279541 };

            Poligonos pol = new Poligonos(xx, yy, 3);

            return pol.contains(lat, lon);
        }
    }
}

