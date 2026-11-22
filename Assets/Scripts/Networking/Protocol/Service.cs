using Functions.Auto;
using System;
using UnityEngine;

public partial class Service
{
    private ISession session = Session_ME.gI();
    protected static Service instance;
    // xx: Xx.
    private int xx;
    // yy: Yy.
    private int yy;
    public const sbyte KHAMNGOC = 0;
    // LUYENNGOC: Gem refinement.
    public const sbyte LUYENNGOC = 1;
    // GOTNGOC: Gem polishing.
    public const sbyte GOTNGOC = 2;
    public const sbyte THAONGOC = 3;
}
