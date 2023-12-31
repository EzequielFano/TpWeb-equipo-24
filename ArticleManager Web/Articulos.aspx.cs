﻿using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using System.Collections;


namespace ArticleManager_Web
{
    public partial class Articulos1 : System.Web.UI.Page
    {
        public List<Articulo> ListaArticulos { get; set; }
        static public List<Articulo> ArticulosCarrito { get; set; }
        static public int cantidad { get; set; }
        
        public List<Imagen> ListaImagenes { get; set; }

        public List<int> idArticulo { get; set; }
        public bool filtrado { get; set; }

        public bool session { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            filtrado=Session["Filtrado"] != null ? true : false;
            ListaArticulos = (List<Articulo>)Session["ListaArticulos"];
            session = Session["session"] != null ? (bool)Session["session"] : false;
            if (!filtrado)
            {
            ArticulosNegocio negocio = new ArticulosNegocio();
            ListaArticulos = negocio.TraerListadoSP();
                if (!IsPostBack)
                {
                    rpRepetidor.DataSource = ListaArticulos;
                    rpRepetidor.DataBind();
                }
            }
            else
            {
                rpRepetidor.DataSource = ListaArticulos;
                rpRepetidor.DataBind();
            }
        }

        protected void btnCarrito_Click(object sender, EventArgs e)
        {

            string valor = ((Button)sender).CommandArgument;
           
            ArticulosNegocio negocio = new ArticulosNegocio();

            List<Articulo> auxArticulo = negocio.TraerListadoCompletoxId(int.Parse(valor));
            if (ArticulosCarrito == null)
            {
                ArticulosCarrito = new List<Articulo>();
            }
            ArticulosCarrito.Add(auxArticulo[0]);

            if (idArticulo == null)
            {
                idArticulo = new List<int>();
            }
            idArticulo.Add(int.Parse(valor));
            cantidad = ArticulosCarrito.Count();
            Session.Add("ArticulosCarrito", ArticulosCarrito);
            Session.Add("idArticulo", idArticulo);
            Session.Add("cantidad", cantidad);
            Response.Redirect("Articulos.aspx", false);
            
        }

        protected void btnLogueate_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx",false);
        }               

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            List<Articulo> lista = new List<Articulo>();
            List<Articulo> auxArticulo = negocio.TraerListadoSP();
            string busqueda = txtBuscador.Text;
            if (busqueda.Length > 0)
            {
                lista = auxArticulo.FindAll(x => x.NombreArticulo.ToUpper().Contains(busqueda.ToUpper())
                || x.CodigoArticulo.ToUpper().Contains(busqueda.ToUpper())
                || x.Descripcion.ToUpper().Contains(busqueda.ToUpper())
                || x.Marca.Descripcion.ToUpper().Contains(busqueda.ToUpper())
                || x.Categoria.Descripcion.ToUpper().Contains(busqueda.ToUpper()));
            }
            else
            {
                lista = auxArticulo;
            }
            filtrado = true;
            ListaArticulos = lista;
            Session.Add("Filtrado", filtrado);
            Session.Add("ListaArticulos", ListaArticulos);
            Response.Redirect("Articulos.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            List<Articulo> lista = new List<Articulo>();
            List<Articulo> auxArticulo = negocio.TraerListadoSP();
            lista= auxArticulo;
            filtrado = false;
            ListaArticulos = lista;
            Session.Add("Filtrado", filtrado);
            Session.Add("ListaArticulos", ListaArticulos);
            Response.Redirect("Articulos.aspx");

        }
    }
}