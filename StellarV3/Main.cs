using ClarityLib;
using Il2CppVRC.Core;
using MelonLoader;
using StellarV3.Features.Exploits;
using StellarV3.Features.Movement;
using StellarV3External.Features.Visuals;
using StellarV3External.Menus;
using StellarV3External.SDK;
using System.Collections;
using UnityEngine;
using VRC;

[assembly: MelonInfo(typeof(Main), "StellarV3External", "1.1.3", "4gottenmemory", "https://discord.gg/myuWgYP8WS")]
[assembly: MelonGame("VRChat", "VRChat")]

public class Main : MelonMod
{
    private static int selectedTab = 0;
    public static bool showMenu = false;
    public static Player selectedPlayer = null;

    private static Texture2D resizeIcon;

    private static Rect windowRect = new Rect(10, 10, 500, 700);
    private static bool isDragging = false;
    private static Vector2 dragOffset;

    private static bool isResizing = false;
    private static Vector2 resizeStart;

    private const int tabWidth = 80;
    private const int tabHeight = 25;
    private const int tabSpacing = 10;
    private static readonly string[] tabNames = { "Movement", "Visuals", "World", "Players", "Exploit" };

    public override void OnApplicationStart()
    {
        Logs.Initialize();
        Logging.InitConsole();

        ClarityLib.Logs.Log("Init Patches", LType.Info.ToString(), Logging.GetColor(LType.Info), System.ConsoleColor.Cyan, "Stellar");
        Task.Run(() => StellarV3External.SDK.Patching.Patch.Init());

        ClarityLib.Logs.Log("Loading StellarV3", LType.Info.ToString(), Logging.GetColor(LType.Info), System.ConsoleColor.Cyan, "Stellar");
        MelonCoroutines.Start(WaitForUser());

        ClarityLib.Logs.Log("Join The Discord: https://discord.gg/myuWgYP8WS", LType.Info.ToString(), Logging.GetColor(LType.Info), System.ConsoleColor.Cyan, "Stellar");
    }

    private static IEnumerator WaitForUser()
    {
        while (APIUser.CurrentUser == null)
            yield return null;

        VRCUserName = APIUser.CurrentUser.displayName;

        ClarityLib.Logs.Log("Waiting for Canvas_QuickMenu(Clone)", LType.Info.ToString(), Logging.GetColor(LType.Info), System.ConsoleColor.Cyan, "Stellar");
        while (Camera.main == null || GameObject.Find("Canvas_QuickMenu(Clone)") == null)
            yield return null;

        ClarityLib.Logs.Log("Quick Menu found", LType.Info.ToString(), Logging.GetColor(LType.Info), System.ConsoleColor.Cyan, "Stellar");
        if (GameObject.Find("Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_QM_Launchpad"))
        {
            ClarityLib.Logs.Log("Cleaning QM", LType.Info.ToString(), Logging.GetColor(LType.Info), System.ConsoleColor.Cyan, "Stellar");
            GameObject gameObject1 = GameObject.Find("Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_QM_Launchpad/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners");
            GameObject gameObject2 = GameObject.Find("Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_QM_Launchpad/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners");
            gameObject1.SetActive(false);
            gameObject2.SetActive(false);

            ClarityLib.Logs.Log("Quick Menu Cleaned", LType.Info.ToString(), Logging.GetColor(LType.Info), System.ConsoleColor.Cyan, "Stellar");
        }
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Insert) || Input.GetKeyDown(KeyCode.F5))
            ToggleMenu();

        Spoofer.NameSpoof();
        MovementGUI.Update();
        ClickTP.Update();
    }


    public override void OnGUI()
    {
        InfoGUI();

        MainGUI();

        NameESP.OnGUI();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        WorldHacksGUI.Initialize(sceneName);
    }

    #region GUI
    public static string VRCUserName = "";
    public static void InfoGUI()
    {
        string username = VRCUserName ?? "Unknown User";

        GUIStyle textStyle = new GUIStyle
        {
            fontSize = 14,
            normal = { textColor = Color.white },
            richText = true
        };

        GUI.Label(new Rect(10, 10, 300, 30), $"<b>Stellar V3</b>", textStyle);
        GUI.Label(new Rect(10, 30, 300, 30), $"<b>User: {username}</b>", textStyle);
        GUI.Label(new Rect(10, 50, 300, 30), $"<b>Made By 4gottenmemory</b>", textStyle);
    }

    public static void MainGUI()
    {
        if (!showMenu) return;

        GUI.backgroundColor = Color.black;
        windowRect = GUI.Window(0, windowRect, (GUI.WindowFunction)DrawWindow, "<color=#FFFFFF>Stellar V3</color>  <color=#FFFFFF>https://discord.gg/myuWgYP8WS</color>");
    }

    private static void DrawWindow(int id)
    {
        HandleDragging();
        HandleResizing();

        for (int i = 0; i < tabNames.Length; i++)
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            string label = (selectedTab == i) ? $"<color=gray><b>{tabNames[i]}</b></color>" : tabNames[i];
            if (GUI.Button(new Rect(20 + i * (tabWidth + tabSpacing), 40, tabWidth, tabHeight), label))
                selectedTab = i;
        }

        GUI.contentColor = Color.white;
        GUI.backgroundColor = Color.black;

        switch (selectedTab)
        {
            case 0: StellarV3External.Menus.MovementGUI.Menu(); break;
            case 1: StellarV3External.Menus.VisualGUI.Menu(); break;
            case 2: StellarV3External.Menus.WorldHacksGUI.Menu(); break;
            case 3: StellarV3External.Menus.PlayersGUI.Menu(); break;
            case 4: StellarV3External.Menus.ExploitGUI.Menu(); break;
        }

        GUI.DragWindow(new Rect(0, 0, windowRect.width, 30));
    }

    #region GUIHelpers
    public static void ToggleMenu()
    {
        if (resizeIcon == null)
        {
            ResizeHelper();
        }

        showMenu = !showMenu;
    }

    private static void HandleDragging()
    {
        var dragArea = new Rect(0, 0, windowRect.width, 30);

        if (Event.current.type == EventType.MouseDown && dragArea.Contains(Event.current.mousePosition))
        {
            isDragging = true;
            dragOffset = Event.current.mousePosition;
            Event.current.Use();
        }

        if (isDragging && Event.current.type == EventType.MouseDrag)
        {
            windowRect.position += Event.current.mousePosition - dragOffset;
            Event.current.Use();
        }

        if (Event.current.type == EventType.MouseUp)
        {
            isDragging = false;
        }
    }

    private static void HandleResizing()
    {
        Rect resizeArea = new Rect(windowRect.width - 15, windowRect.height - 15, 15, 15);

        if (resizeIcon != null)
            GUI.DrawTexture(resizeArea, resizeIcon);
        else
            GUI.DrawTexture(resizeArea, Texture2D.whiteTexture);

        if (Event.current.type == EventType.MouseDown && resizeArea.Contains(Event.current.mousePosition))
        {
            isResizing = true;
            resizeStart = Event.current.mousePosition;
            Event.current.Use();
        }

        if (isResizing && Event.current.type == EventType.MouseDrag)
        {
            Vector2 resizeDelta = Event.current.mousePosition - resizeStart;
            windowRect.width = Mathf.Max(300, windowRect.width + resizeDelta.x);
            windowRect.height = Mathf.Max(300, windowRect.height + resizeDelta.y);
            resizeStart = Event.current.mousePosition;
            Event.current.Use();
        }

        if (Event.current.type == EventType.MouseUp)
        {
            isResizing = false;
        }
    }

    private static void ResizeHelper()
    {
        int size = 12;
        resizeIcon = new Texture2D(size, size, TextureFormat.RGBA32, false);
        resizeIcon.wrapMode = TextureWrapMode.Clamp;

        Color transparent = new Color(0, 0, 0, 0);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                resizeIcon.SetPixel(x, y, transparent);
            }
        }

        Color lineColor = new Color(1f, 1f, 1f, 0.7f);
        for (int i = 0; i < size; i += 3)
        {
            for (int j = 0; j < 2; j++)
            {
                int x = size - i - j - 1;
                if (x >= 0 && x < size)
                {
                    for (int y = size - i - j - 1; y < size; y++)
                    {
                        if (y >= 0 && y < size)
                            resizeIcon.SetPixel(x, y, lineColor);
                    }
                }
            }
        }

        resizeIcon.Apply();
    }

    #endregion

    #endregion
}