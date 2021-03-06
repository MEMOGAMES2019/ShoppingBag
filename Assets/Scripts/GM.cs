﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using RAGE.Analytics;

public class GM : MonoBehaviour
{
    #region Atributos

    /// <summary>
    /// Manejado general del juego.
    /// </summary>
    public static GM Gm { get; set; }

    /// <summary>
    /// Lista de la compra.
    /// </summary>
    public List<ShopObject> ShopList { get; set; } = new List<ShopObject>();

    /// <summary>
    /// Nombre del objeto de la lista actual.
    /// </summary>
    public ShopObject CurrentObject { get; set; }

    /// <summary>
    /// Lista de objetos seleccionados correctamente.
    /// </summary>
    private List<ShopObject> CorrectList { get; set; } = new List<ShopObject>();

    /// <summary>
    /// Lista de objetos seleccionados erroneamente.
    /// </summary>
    private List<ShopObject> WrongList { get; set; } = new List<ShopObject>();

    public string Level { get; set; }

    #endregion

    #region Eventos

    private void Awake()
    {
        if (Gm == null)
        {
            Gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Gm != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        PlayerPrefs.DeleteAll();
    }

    #endregion

    #region Métodos públicos

    /// <summary>
    /// Carga la siguiente escena.
    /// </summary>
    /// <param name="scene">Nombre de la escena a la que se va a cambiar.</param>
    public void LoadScene(string scene)
    {
        Tracker.T.Accessible.Accessed(scene, AccessibleTracker.Accessible.Screen);
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Método para el botón salir del menú.
    /// </summary>
    public void DoExitGame()
    {
        Tracker.T.setVar("ExitGame", 1);
        Application.Quit();
    }

    /// <summary>
    /// Inicializa/Resetea las listas.
    /// </summary>
    public void InitList()
    {
        Tracker.T.setVar("InitList", 1);
        ShopList.Clear();
        CorrectList.Clear();
        WrongList.Clear();
    }

    /// <summary>
    /// Recoge el siguiente objeto a seleccionar. Si ya no quedan objetos saltará una excepción.
    /// </summary>
    public void NextObject()
    {
        if (ShopList.Count == 0) throw new Exception();

        CurrentObject = new ShopObject(ShopList[0].Name, ShopList[0].ShopType);
        ShopList.RemoveAt(0);
    }

    /// <summary>
    /// Se ha acertado el objeto con su respectiva tienda.
    /// </summary>
    /// <param name="obj">Objeto acertado.</param>
    public void CorrectShop(ShopObject obj)
    {
        Tracker.T.setVar(obj.Name, 1);
        Tracker.T.setVar("CorrectList", 1);
        CorrectList.Add(obj);
    }

    /// <summary>
    /// Se ha equivocado con el objeto con otra tienda.
    /// </summary>
    /// <param name="obj">Objeto erróneo.</param>
    public void WrongShop(ShopObject obj)
    {
        Tracker.T.setVar(obj.Name, 1);
        Tracker.T.setVar("WrongList", 1);
        WrongList.Add(obj);
    }

    public string CorrectListResult()
    {
        if (CorrectList.Count == 0) return null;

        StringBuilder sb = new StringBuilder();
        CorrectList.ForEach(obj =>
        {
            sb.AppendLine(string.Concat("- ", obj.Name));
        });
        return sb.ToString();
    }

    public void PutResult()
    {
        double maxResult = CorrectList.Count + WrongList.Count;
        if (!PlayerPrefs.HasKey(Level + "Star") || (PlayerPrefs.HasKey(Level + "Star") && PlayerPrefs.GetInt(Level + "Star") < CorrectList.Count))
        {
            if (CorrectList.Count == maxResult)
            {
                PlayerPrefs.SetInt(Level, 1);
                PlayerPrefs.SetInt(Level + "Star", 3);
                Tracker.T.setVar("Estrellas " + Level, 3);
                Tracker.T.Completable.Completed(Level, CompletableTracker.Completable.Level, true);
            }
            else if (CorrectList.Count < maxResult && CorrectList.Count >= (maxResult * (2.0 / 3.0)))
            {
                PlayerPrefs.SetInt(Level, 1);
                PlayerPrefs.SetInt(Level + "Star", 2);
                Tracker.T.setVar("Estrellas " + Level, 2);
                Tracker.T.Completable.Completed(Level, CompletableTracker.Completable.Level, true);
            }
            else if (CorrectList.Count < (maxResult * (2.0 / 3.0)) && CorrectList.Count >= (maxResult * (1.0 / 3.0)))
            {
                PlayerPrefs.SetInt(Level, 0);
                PlayerPrefs.SetInt(Level + "Star", 1);
                Tracker.T.setVar("Estrellas " + Level, 1);
                Tracker.T.Completable.Completed(Level, CompletableTracker.Completable.Level, false);
            }
            else
            {
                PlayerPrefs.SetInt(Level, 0);
                PlayerPrefs.SetInt(Level + "Star", 0);
                Tracker.T.setVar("Estrellas " + Level, 0);
                Tracker.T.Completable.Completed(Level, CompletableTracker.Completable.Level, false);
            }            

            
        }
    }

    public string WrongListResult()
    {
        if (WrongList.Count == 0) return null;

        StringBuilder sb = new StringBuilder();
        WrongList.ForEach(obj =>
        {
            sb.AppendLine(string.Concat("- ", obj.Name));
        });
        return sb.ToString();
    }

    #endregion

}