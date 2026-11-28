using Functions.Auto;
using System;
using System.Collections;

public partial class Char : MainObject
{
    public bool isPantEffclonechar;
    public short[] idpartThoitrang = new short[4]
    {
        -1,
        -1,
        -1,
        -1
    };
    public bool isHuman;
    public bool isNhanban;
    public bool isCaptcha;
    private int tickEffWolf;
    private int timeBocdau;
    public long lastUpdateTime;
    public const sbyte A_STAND = 1;
    public const sbyte A_RUN = 2;
    public const sbyte A_JUMP = 3;
    public const sbyte A_FALL = 4;
    public const sbyte A_DEADFLY = 5;
    public const sbyte A_NOTHING = 6;
    public const sbyte A_ATTK = 7;
    public const sbyte A_INJURE = 8;
    public const sbyte A_AUTOJUMP = 9;
    public const sbyte A_WATERRUN = 10;
    public const sbyte A_WATERDOWN = 11;
    public const sbyte SKILL_STAND = 12;
    public const sbyte SKILL_FALL = 13;
    public const sbyte A_DEAD = 14;
    public const sbyte A_HIDE = 15;
    public ChatPopup chatPopup;
    public long cEXP;
    public long cExpDown;
    public long cExpR;
    public int lcx;
    public int lcy;
    // cx: Character horizontal position in map pixels.
    public int cx = 24;
    // cy: Character vertical position in map pixels.
    public int cy = 24;
    public int cvx;
    public int cvy;
    public int cp1;
    public int cp2;
    public int cp3;
    public int statusMe = 5;
    public int cdir = 1;
    public int charID;
    public int cgender;
    public int ctaskId;
    public int cBonusSpeed;
    public int cspeed;
    public int ccurrentAttack;
    public int cdame;
    public int cdameDown;
    public int clevel;
    // cMP: Character mana points.
    public int cMP;
    public int cMaxMP;
    // cHP: Character hit points.
    public int cHP;
    public int cHpNew;
    public int cMaxHP;
    public int cMaxEXP;
    public int HPShow;
    public int xReload;
    public int yReload;
    public int cyStartFall;
    public int saveStatus;
    public int eff5BuffHp;
    public int eff5BuffMp;
    public int autoUpHp;
    public int pPoint;
    public int sPoint;
    public int pointUydanh;
    public int pointNon;
    public int pointVukhi;
    public int pointAo;
    public int pointLien;
    public int pointGangtay;
    public int pointNhan;
    public int pointQuan;
    public int pointNgocboi;
    public int pointGiay;
    public int pointPhu;
    public int pointTinhTu;
    public int countFinishDay;
    public int countLoopBoos;
    public int limitTiemnangso;
    public int limitKynangso;
    public int limitPhongLoi;
    public int limitBangHoa;
    public int countPB;
    public int[] potential = new int[4];
    public int bijuuPoint;
    public int bijuuSkillPoint;
    public int[] bijuupotential = new int[4];
    public MyVector vSkillBijuu = new MyVector();
    public string cName;
    // cw: Cw.
    public int cw = 22;
    // ch: Ch.
    public int ch = 32;
    public int chw = 11;
    public int chh = 16;
    public bool canJumpHigh = true;
    public bool cmtoChar;
    // me: Acting character.
    public bool me;
    public bool cchistlast;
    public bool isAttack;
    public bool isAttFly;
    // cf: Cf.
    public int cf;
    public int tick;
    public static bool fallAttack;
    public bool isJump;
    public bool autoFall;
    public bool isMoto;
    public bool isWolf;
    public bool isMotoBehind;
    public bool isBocdau;
    public bool isNewMount;
    // xu: Xu currency.
    public int xu;
    public int xuInBox;
    // yen: Yen currency.
    public int yen;
    public int gold_lock;
    // luong: Premium currency.
    public int luong;
    public NClass nClass;
    public MyVector vSkill = new MyVector();
    public MyVector vSkillFight = new MyVector();
    public MyVector vEff = new MyVector();
    public MyVector vDomsang = new MyVector();
    public Skill myskill;
    public Task taskMaint;
    public bool paintName = true;
    public Item[] arrItemBag;
    public Item[] arrItemBox;
    public Item[] arrItemBody;
    public Item[] arrItemMounts = new Item[5];
    public Item[] arrItemViThu = new Item[5];
    public sbyte[] mountsPoint = new sbyte[10];
    public Item[] arrItemBijuu = new Item[5];
    public short cResFire;
    public short cResIce;
    public short cResWind;
    public short cMiss;
    public short cExactly;
    public short cFatal;
    public sbyte cPk;
    public sbyte cTypePk;
    public short cReactDame;
    public short sysUp;
    public short sysDown;
    public int skillTemplateId;
    public Mob mobFocus;
    public Mob mobMe;
    public Mob mobVithu;
    public Npc npcFocus;
    public Char charFocus;
    public ItemMap itemFocus;
    public MyVector focus = new MyVector();
    public Mob[] attMobs;
    public Char[] attChars;
    public short[] moveFast;
    public int testCharId = -9999;
    public int killCharId = -9999;
    public sbyte resultTest;
    public int countKill;
    public int countKillMax;
    public int tickCoat;
    public int tickEffmoto;
    public int tickEffFireW;
    public bool isInvisible;
    public long timeStartBlink;
    public static bool isAHP;
    public static bool isAMP;
    public static bool isAFood;
    public static bool isABuff;
    public static bool isAResuscitate;
    public static bool isAPickYen;
    public static bool isAPickYHM;
    public static bool isAPickYHMS;
    public static bool isANoPick;
    public static bool isAFocusDie;
    public static int aHpValue = 20;
    public static int aMpValue = 20;
    public static int aFoodValue = 70;
    public static int aCID;
    public long lastTimeUseSkill;
    public const sbyte PK_NORMAL = 0;
    public static sbyte PK_NHOM = 1;
    public const sbyte PK_PHE = 1;
    public const sbyte PK_BANG = 2;
    public const sbyte PK_DOSAT = 3;
    public static sbyte PK_PHE1 = 4;
    public static sbyte PK_PHE2 = 5;
    public static sbyte PK_PHE3 = 6;
    public static sbyte PK_PHE4 = 7;
    public MyVector taskOrders = new MyVector();
    public string cClanName = string.Empty;
    public sbyte ctypeClan;
    public static Clan clan;
    public static int pointPB;
    public static int pointChienTruong;
    public long timeSummon;
    public long timeChatReturn;
    public bool isPolycy;
    public static readonly int[][][] CharInfo = new int[30][][]
    {
        new int[4][]
        {
            new int[3]
            {
                0,
                -10,
                32
            },
            new int[3]
            {
                1,
                -7,
                7
            },
            new int[3]
            {
                1,
                -11,
                15
            },
            new int[3]
            {
                1,
                -9,
                45
            }
        },
        new int[4][]
        {
            new int[3]
            {
                0,
                -10,
                33
            },
            new int[3]
            {
                1,
                -7,
                7
            },
            new int[3]
            {
                1,
                -11,
                16
            },
            new int[3]
            {
                1,
                -9,
                46
            }
        },
        new int[4][]
        {
            new int[3]
            {
                1,
                -10,
                33
            },
            new int[3]
            {
                2,
                -10,
                11
            },
            new int[3]
            {
                2,
                -9,
                16
            },
            new int[3]
            {
                1,
                -12,
                49
            }
        },
        new int[4][]
        {
            new int[3]
            {
                1,
                -10,
                32
            },
            new int[3]
            {
                3,
                -11,
                9
            },
            new int[3]
            {
                3,
                -11,
                16
            },
            new int[3]
            {
                1,
                -13,
                47
            }
        },
        new int[4][]
        {
            new int[3]
            {
                1,
                -10,
                34
            },
            new int[3]
            {
                4,
                -9,
                9
            },
            new int[3]
            {
                4,
                -8,
                16
            },
            new int[3]
            {
                1,
                -12,
                47
            }
        },
        new int[4][]
        {
            new int[3]
            {
                1,
                -10,
                34
            },
            new int[3]
            {
                5,
                -11,
                11
            },
            new int[3]
            {
                5,
                -10,
                17
            },
            new int[3]
            {
                1,
                -13,
                49
            }
        },
        new int[4][]
        {
            new int[3]
            {
                1,
                -10,
                33
            },
            new int[3]
            {
                6,
                -9,
                9
            },
            new int[3]
            {
                6,
                -8,
                16
            },
            new int[3]
            {
                1,
                -12,
                47
            }
        },
        new int[4][]
        {
            new int[3]
            {
                0,
                -9,
                36
            },
            new int[3]
            {
                7,
                -5,
                15
            },
            new int[3]
            {
                7,
                -10,
                21
            },
            new int[3]
            {
                1,
                -8,
                49
            }
        },
        new int[4][]
        {
            new int[3]
            {
                4,
                -13,
                26
            },
            new int[3],
            new int[3],
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                5,
                -13,
                25
            },
            new int[3],
            new int[3],
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                6,
                -12,
                26
            },
            new int[3],
            new int[3],
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                7,
                -13,
                25
            },
            new int[3],
            new int[3],
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                0,
                -9,
                35
            },
            new int[3]
            {
                8,
                -4,
                13
            },
            new int[3]
            {
                8,
                -14,
                27
            },
            new int[3]
            {
                1,
                -9,
                49
            }
        },
        new int[4][]
        {
            new int[3]
            {
                0,
                -9,
                31
            },
            new int[3]
            {
                9,
                -11,
                8
            },
            new int[3]
            {
                10,
                -10,
                17
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -7,
                33
            },
            new int[3]
            {
                9,
                -11,
                8
            },
            new int[3]
            {
                11,
                -8,
                15
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -8,
                32
            },
            new int[3]
            {
                9,
                -11,
                8
            },
            new int[3]
            {
                12,
                -8,
                14
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -7,
                32
            },
            new int[3]
            {
                9,
                -11,
                8
            },
            new int[3]
            {
                13,
                -12,
                15
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                0,
                -11,
                31
            },
            new int[3]
            {
                9,
                -11,
                8
            },
            new int[3]
            {
                14,
                -15,
                18
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -9,
                32
            },
            new int[3]
            {
                9,
                -11,
                8
            },
            new int[3]
            {
                15,
                -13,
                19
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -9,
                31
            },
            new int[3]
            {
                9,
                -11,
                8
            },
            new int[3]
            {
                16,
                -7,
                22
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -9,
                32
            },
            new int[3]
            {
                9,
                -11,
                8
            },
            new int[3]
            {
                17,
                -11,
                18
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                3,
                -12,
                34
            },
            new int[3]
            {
                8,
                -4,
                13
            },
            new int[3]
            {
                8,
                -15,
                25
            },
            new int[3]
            {
                1,
                -10,
                46
            }
        },
        new int[4][]
        {
            new int[3]
            {
                0,
                -9,
                32
            },
            new int[3]
            {
                8,
                -4,
                9
            },
            new int[3]
            {
                10,
                -10,
                18
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -7,
                34
            },
            new int[3]
            {
                8,
                -4,
                9
            },
            new int[3]
            {
                11,
                -8,
                16
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -8,
                33
            },
            new int[3]
            {
                8,
                -4,
                9
            },
            new int[3]
            {
                12,
                -8,
                15
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -7,
                33
            },
            new int[3]
            {
                8,
                -4,
                9
            },
            new int[3]
            {
                13,
                -12,
                16
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                0,
                -11,
                32
            },
            new int[3]
            {
                7,
                -5,
                9
            },
            new int[3]
            {
                14,
                -15,
                19
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -9,
                33
            },
            new int[3]
            {
                7,
                -5,
                9
            },
            new int[3]
            {
                15,
                -13,
                20
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -9,
                32
            },
            new int[3]
            {
                7,
                -5,
                9
            },
            new int[3]
            {
                16,
                -7,
                23
            },
            new int[3]
        },
        new int[4][]
        {
            new int[3]
            {
                2,
                -9,
                33
            },
            new int[3]
            {
                7,
                -5,
                9
            },
            new int[3]
            {
                17,
                -11,
                19
            },
            new int[3]
        }
    };
    public int[] CHAR_WEAPONX = new int[11]
    {
        -2,
        -6,
        22,
        21,
        19,
        22,
        10,
        -2,
        -2,
        5,
        19
    };
    public int[] CHAR_WEAPONY = new int[11]
    {
        9,
        22,
        25,
        17,
        26,
        37,
        36,
        49,
        50,
        52,
        36
    };
    private static Char myChar;
    public static int[] listAttack;
    public static int[][] listIonC;
    public int cvyJump;
    private int indexUseSkill = -1;
    // cxSend: Last reported horizontal position.
    public int cxSend;
    // cySend: Last reported vertical position.
    public int cySend;
    public int cdirSend = 1;
    public int cxFocus;
    public int cyFocus;
    public int cxMoveLast;
    public int cyMoveLast;
    public int cactFirst = 5;
    public MyVector vMovePoints = new MyVector();
    // flag: Branch/result flag for the current operation.
    public static bool flag;
    public static bool ischangingMap;
    public static bool isLockKey;
    public bool isLockMove;
    public bool isLockAttack;
    public bool isBlinking;
    public MovePoint currentMovePoint;
    public int bom;
    public int count;
    public bool isEffBatTu;
    public long lastUseHP = mSystem.currentTimeMillis();
    public bool beginSound;
    public long timeBeGinRun;
    public int vitaWolf;
    public long timeSendmove;
    public static bool isSendMove = true;
    public long time;
    private int cX0;
    private int cY0;
    private int fbyhorse;
    private int FramecharRideHorse;
    private int inAir;
    private long currMove;
    public bool isLastMove;
    public static sbyte[] locate = new sbyte[12]
    {
        0,
        0,
        0,
        -1,
        -1,
        -1,
        -2,
        -2,
        -2,
        -1,
        -1,
        -1
    };
    private long timeLastCheck;
    public long timelastSendmove;
    public static short delaySendmove = 250;
    public short head;
    public short leg;
    public short body;
    // wp: Wp.
    public short wp;
    public short coat = -1;
    public short glove = -1;
    public int indexEff = -1;
    public int indexEffTask = -1;
    // eff: Effect.
    public EffectCharPaint eff;
    public EffectCharPaint effTask;
    public int indexSkill;
    private bool skillActionTriggered; // one hit/buff action per animation, independent of render FPS
    // i0: I0.
    public int i0;
    // i1: I1.
    public int i1;
    // i2: I2.
    public int i2;
    public int dx0;
    public int dx1;
    public int dx2;
    public int dy0;
    public int dy1;
    public int dy2;
    public EffectCharPaint eff0;
    public EffectCharPaint eff1;
    public EffectCharPaint eff2;
    public Arrow arr;
    public SkillPaint skillPaint;
    public EffectPaint[] effPaints;
    public int sType;
    public sbyte isInjure;
    public static mHashtable ALL_PART_EFF = new mHashtable();
    private int heightCharName;
    private int dxHead;
    private int dyHead;
    private int dxBody;
    private int dyBody;
    private int tickWolf;
    // dy: Vertical difference or offset.
    private int dy;
    private int hdx;
    private int hdy;
    private int[] idWolfW = new int[4]
    {
        1715,
        1737,
        1714,
        1738
    };
    public int FrameName;
    public int fName;
    public int FrameRank;
    public int fRank;
    public int FrameMatna;
    public int fMatNa;
    public int FrameBienHinh;
    public int fBienhinh;
    public int FrameWeaPone;
    public int fWeapone;
    public int FrameSusan;
    public int fsusan;
    public int FrameLeg;
    public int fLeg;
    public int FrameBody;
    public int fBody;
    public int FrameHair;
    public int fHair;
    public short ID_Body = -1;
    public short ID_PP = -1;
    public short ID_HAIR = -1;
    public short ID_LEG = -1;
    public short ID_HORSE = -1;
    public short ID_NAME = -1;
    public short ID_RANK = -1;
    public short ID_MAT_NA = -1;
    public short ID_Bien_Hinh = -1;
    public short ID_WEA_PONE = -1;
    public short ID_SUSANO = -1;
    public int FramePP;
    public int fPP;
    public int FrameHorse;
    public int fho;
    private sbyte[] ActionHorse;
    public sbyte ColorName;
    public static mFont[] color_font_name = new mFont[4]
    {
        mFont.tahoma_7_white,
        mFont.tahoma_7_blue1,
        mFont.tahoma_7b_purple,
        mFont.tahoma_7b_yellow
    };
    public sbyte inDexActionn;
    private int fHorseUI;
    private sbyte[] fhorseui;
    private int fhorsetrangbi;
    public short[] fRun_PP = new short[10]
    {
        2,
        2,
        3,
        3,
        4,
        4,
        5,
        5,
        6,
        6
    };
    private int FbodyUI;
    private int FlegUI;
    private int fHeadUI;
    private int fPPUI;
    private byte fWeaponUI;
    private byte fMatNaUI;
    private int dynewhhorse;
    private int dxnewhorse;
    private sbyte frameFix = 2;
    public static bool isManualFocus = false;
    private int nInjure;
    public short wdx;
    public short wdy;
    public bool isDirtyPostion;
    public Skill lastNormalSkill;
    public bool currentFireByShortcut;
    private int EffdefautX;
    private int EffdefautY;
    private int Effx;
    private int Effy;
    private int tickEffyesWolfmove;
    private int EffdefautX1;
    private int EffdefautY1;
    private bool isPaintNo = true;
    // img: Image.
    private FrameImage img;
    private static Image imgNo;
    // f: Current field/value.
    private int f;
    private int countNo;
    private int dyNo;
    private int dxNo;
    private int[][] frameMount = new int[6][]
    {
        new int[2]
        {
            3049,
            3050
        },
        new int[6]
        {
            3051,
            3051,
            3052,
            3052,
            3053,
            3053
        },
        new int[1]
        {
            3054
        },
        new int[1]
        {
            3055
        },
        new int[1]
        {
            3056
        },
        new int[6]
        {
            3049,
            3049,
            3049,
            3050,
            3050,
            3050
        }
    };
    private int tickNewMount;
    private int fNewMount;
    private int dxMount;
    private int dyMount;
    /// <summary>Initialize character state and its supplied dependencies.</summary>
    public Char()
    {
        statusMe = 6;
    }

    /// <summary>Initialize character state and its supplied dependencies.</summary>
    static Char()
    {
        Handle.LoadAuto();
    }

    /// <summary>Return sys.</summary>
    public int getSys()
    {
        if (nClass.classId == 1 || nClass.classId == 2)
        {
            return 1;
        }

        if (nClass.classId == 3 || nClass.classId == 4)
        {
            return 2;
        }

        if (nClass.classId == 5 || nClass.classId == 6)
        {
            return 3;
        }

        return 0;
    }

    /// <summary>Return speed.</summary>
    public int getSpeed()
    {
        if (Handle.isHackGiay)
        {
            return Handle.HackGiay;
        }

        if (isWolf || isMoto) // cái này là dành cho thú cưỡi 
        {
            return cspeed + 2;
        }

        return cspeed;
    }

    /// <summary>Check whether use long range weapon applies to the supplied state.</summary>
    public bool isUseLongRangeWeapon()
    {
        return nClass.classId == 2 || nClass.classId == 4 || nClass.classId == 6;
    }

    /// <summary>Return my character.</summary>
    public static Char getMyChar()
    {
        if (myChar == null)
        {
            myChar = new Char();
            myChar.me = true;
            myChar.cmtoChar = true;
            myChar.timelastSendmove = mSystem.currentTimeMillis();
        }

        return myChar;
    }

    /// <summary>Clear my character.</summary>
    public static void clearMyChar()
    {
        isABuff = (isAFocusDie = (isAFood = (isAHP = (isAMP = (isANoPick = (isAPickYen = (isAPickYHM = (isAPickYHMS = (isAResuscitate = false)))))))));
        myChar = null;
    }

    /// <summary>Compute solg inventory.</summary>
    /// <remarks>Locals: i1: I1; i2: I2.</remarks>
    public static int solg_hanhtrang()
    {
        Item[] arr = Char.getMyChar().arrItemBag;
        int i1 = 0;
        for (int i2 = 0; i2 < arr.Length; i2++)
        {
            if (arr[i2] == null)
            {
                i1++;
            }
        }

        return i1;
    }

    /// <summary>Check stone.</summary>
    /// <remarks>Locals: var0: Intermediate value retained under its original legacy name; var1: Intermediate value retained under its original legacy name; var2: Intermediate value retained under its original legacy name; var3: Item instance being inspected or transferred.</remarks>
    public static int checkStone()
    {
        Item[] var0 = getMyChar().arrItemBag;
        int var1 = 0;
        for (int var2 = 0; var2 < var0.Length; ++var2)
        {
            Item var3;
            if ((var3 = var0[var2]) != null && var3.template.type == 26 && var3.template.id <= 4)
            {
                var1 += GameScr.upClothe[var3.template.id];
            }
        }

        return var1;
    }

    /// <summary>Read param.</summary>
    /// <param name="msg">Framed protocol message.</param>
    /// <param name="pos">Retained for call compatibility; not read by this implementation.</param>
    public void readParam(Message msg, string pos)
    {
        try
        {
            cspeed = msg.reader().readByte();
            cMaxHP = msg.reader().readInt();
            cMaxMP = msg.reader().readInt();
        }
        catch (Exception)
        {
        }
    }

    /// <summary>Return distance.</summary>
    /// <param name="x">Horizontal coordinate in the caller&#x27;s coordinate space.</param>
    /// <param name="x2">X2.</param>
    public static int getDistance(int x, int x2)
    {
        return Res.abs(x - x2);
    }

    /// <summary>Find nearby standable terrain and move there; -1 coordinates use the local character position.</summary>
    /// <param name="paramInt1">Horizontal map position in pixels.</param>
    /// <param name="paramInt2">Vertical map position in pixels.</param>
    /// <remarks>Locals: array: Entry selected from the source collection.</remarks>
    public static bool b(int paramInt1, int paramInt2)
    {
        if (paramInt1 == -1)
        {
            paramInt1 = myChar.cx;
        }

        if (paramInt2 == -1)
        {
            paramInt2 = myChar.cy;
        }

        int[] array = new int[2];
        if (TileMap.a(paramInt1, paramInt2, array))
        {
            return Move(array[0], array[1]);
        }

        return false;
    }

    /// <summary>Set attk.</summary>
    public void setAttk()
    {
        cp1++;
        if (cdir == 1)
        {
            if ((TileMap.tileTypeAtPixel(cx + chw, cy - chh) & TileMap.T_LEFT) == TileMap.T_LEFT)
            {
                cvx = 0;
            }
        }
        else if ((TileMap.tileTypeAtPixel(cx - chw, cy - chh) & TileMap.T_RIGHT) == TileMap.T_RIGHT)
        {
            cvx = 0;
        }

        if (cy > ch && TileMap.tileTypeAt(cx, cy - ch, TileMap.T_BOTTOM))
        {
            if (!TileMap.tileTypeAt(cx, cy, TileMap.T_TOP))
            {
                statusMe = 4;
                cp1 = 0;
                cp2 = 0;
                cvy = 1;
            }
            else
            {
                cy = TileMap.tileYofPixel(cy);
            }
        }

        cx += cvx;
        cy += cvy;
        if (cy < 0)
        {
            cy = (cvy = 0);
        }

        if (cvy == 0)
        {
            if ((TileMap.tileTypeAtPixel(cx, cy) & TileMap.T_TOP) != TileMap.T_TOP)
            {
                statusMe = 4;
                cvx = (getSpeed() >> 1) * cdir;
                cp1 = (cp2 = 0);
            }
        }
        else if (cvy < 0)
        {
            cvy++;
            if (cvy == 0)
            {
                cvy = 1;
            }
        }
        else
        {
            if (cvy < 20 && cp1 % 5 == 0)
            {
                cvy++;
            }

            if (cvy > 3)
            {
                cvy = 3;
            }

            if ((TileMap.tileTypeAtPixel(cx, cy + 3) & TileMap.T_TOP) == TileMap.T_TOP && cy <= TileMap.tileXofPixel(cy + 3))
            {
                cvx = (cvy = 0);
                cy = TileMap.tileXofPixel(cy + 3);
            }

            if (TileMap.tileTypeAt(cx, cy, TileMap.T_WATERFLOW) && cy % TileMap.size > 8)
            {
                statusMe = 10;
                cvx = cdir << 1;
                cvy >>= 2;
                cy = TileMap.tileYofPixel(cy) + 12;
                statusMe = 11;
                return;
            }

            if (TileMap.tileTypeAt(cx, cy, TileMap.T_UNDERWATER))
            {
                statusMe = 11;
                return;
            }
        }

        if (cvx > 0)
        {
            cvx--;
        }
        else if (cvx < 0)
        {
            cvx++;
        }
    }

    /// <summary>Perform play sound.</summary>
    /// <param name="skillPaint">Skill paint.</param>
    /// <remarks>Locals: num: Sample drawn from the random-number source; num2: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    private void playSound(SkillPaint skillPaint)
    {
        int num = NinjaUtil.randomNumber(3);
        float num2 = 0f;
        num2 = (me ? 0.8f : 0.4f);
        switch (nClass.classId)
        {
            case 0:
                Sound.play(Sound.MKiemGo, num2);
                break;
            case 1:
                switch (num)
                {
                    case 0:
                        Sound.play(Sound.MKiem, num2);
                        break;
                    case 1:
                        Sound.play(Sound.MKiem2, num2);
                        break;
                    default:
                        Sound.play(Sound.MKiem3, num2);
                        break;
                }

                break;
            case 2:
                Sound.play(Sound.MTieu, num2);
                break;
            case 3:
                Sound.play(Sound.MKunai, num2);
                break;
            case 4:
                switch (num)
                {
                    case 0:
                        Sound.play(Sound.MCung, num2);
                        break;
                    case 1:
                        Sound.play(Sound.MCung2, num2);
                        break;
                    default:
                        Sound.play(Sound.MCung3, num2);
                        break;
                }

                break;
            case 5:
                if (skillPaint.id == 41)
                {
                    Sound.play(Sound.MDao2, num2);
                }
                else if (skillPaint.id == 45)
                {
                    Sound.play(Sound.MDao3, num2);
                }
                else
                {
                    Sound.play(Sound.MDao, num2);
                }

                break;
            case 6:
                Sound.play(Sound.MQuat, num2);
                break;
        }
    }

    /// <summary>Stop character.</summary>
    private void stop()
    {
        statusMe = 6;
        cvx = 0;
        cvy = 0;
        cp1 = (cp2 = 0);
    }

    /// <summary>Compute abs.</summary>
    /// <param name="i">Loop/collection index.</param>
    public static int abs(int i)
    {
        return (i <= 0) ? (-i) : i;
    }

    /// <summary>Set default part.</summary>
    public void setDefaultPart()
    {
        setDefaultWeapon();
        setDefaultBody();
        setDefaultLeg();
    }

    /// <summary>Set default weapon.</summary>
    public void setDefaultWeapon()
    {
        wp = 15;
    }

    /// <summary>Return default head.</summary>
    /// <param name="gender">Gender.</param>
    public Part getDefaultHead(int gender)
    {
        try
        {
            if (gender == 0)
            {
                return GameScr.parts[27];
            }

            return GameScr.parts[2];
        }
        catch (Exception)
        {
        }

        return null;
    }

    /// <summary>Set default body.</summary>
    public void setDefaultBody()
    {
        if (cgender == 0)
        {
            body = 10;
        }
        else
        {
            body = 1;
        }
    }

    /// <summary>Set default leg.</summary>
    public void setDefaultLeg()
    {
        if (cgender == 0)
        {
            leg = 9;
        }
        else
        {
            leg = 0;
        }
    }

    /// <summary>Check whether have wolf applies to the supplied state.</summary>
    public bool isHaveWolf()
    {
        if (ID_HORSE > -1)
        {
            return false;
        }

        if (arrItemMounts != null && arrItemMounts[4] != null && arrItemMounts[4].template != null && (arrItemMounts[4].template.id == 443 || arrItemMounts[4].template.id == 523))
        {
            return true;
        }

        return false;
    }

    /// <summary>Check whether have moto applies to the supplied state.</summary>
    public bool isHaveMoto()
    {
        if (ID_HORSE > -1)
        {
            return false;
        }

        if (arrItemMounts != null && arrItemMounts[4] != null && arrItemMounts[4].template != null && (arrItemMounts[4].template.id == 485 || arrItemMounts[4].template.id == 524))
        {
            return true;
        }

        return false;
    }

    /// <summary>Return class coat.</summary>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained.</remarks>
    public int[] getClassCoat()
    {
        int[] result = null;
        int num = -1;
        if (me)
        {
            if (arrItemBody[12] != null)
            {
                num = arrItemBody[12].template.id;
            }
        }
        else
        {
            num = coat;
        }

        switch (num)
        {
            case -1:
                return null;
            case 420:
                result = ((!isWolf && !isMoto && !isNewMount) ? new int[4]
                {
                    1635,
                    1636,
                    1637,
                    1636
                }

                : new int[4]
                {
                    2029,
                    2030,
                    2031,
                    2030
                }

                );
                break;
            case 421:
                result = ((!isWolf && !isMoto && !isNewMount) ? new int[4]
                {
                    1652,
                    1653,
                    1654,
                    1653
                }

                : new int[4]
                {
                    2035,
                    2036,
                    2037,
                    2036
                }

                );
                break;
            case 422:
                result = ((!isWolf && !isMoto && !isNewMount) ? new int[4]
                {
                    1655,
                    1656,
                    1657,
                    1656
                }

                : new int[4]
                {
                    2032,
                    2033,
                    2034,
                    2033
                }

                );
                break;
        }

        return result;
    }

    /// <summary>Return class color.</summary>
    public int getClassColor()
    {
        int result = 9145227;
        if (nClass.classId == 1 || nClass.classId == 2)
        {
            result = 16711680;
        }
        else if (nClass.classId == 3 || nClass.classId == 4)
        {
            result = 33023;
        }
        else if (nClass.classId == 5 || nClass.classId == 6)
        {
            result = 7443811;
        }

        return result;
    }

    /// <summary>Compute character check.</summary>
    /// <remarks>Locals: flag: Branch/result flag for the current operation.</remarks>
    public string CharCheck()
    {
        bool flag = !this.cName.StartsWith("[");
        string result;
        if (flag)
        {
            result = "";
        }
        else
        {
            result = this.cName.Substring(0, this.cName.IndexOf("]") + 1);
        }

        return result;
    }

    /// <summary>Compute hanh tinh.</summary>
    /// <remarks>Locals: flag: Branch/result flag for the current operation.</remarks>
    public string HanhTinh()
    {
        bool flag = this.cgender == 0;
        string result;
        if (flag)
        {
            result = "Boy";
        }
        else
        {
            result = "Girl";
        }

        return result;
    }

    /// <summary>Compute getys.</summary>
    public int getys()
    {
        int result = 0;
        if (isMoto)
        {
            result = ((arrItemMounts[4].template.id != 485) ? 6 : 4);
        }

        if (isNewMount)
        {
            result = 2;
            if (statusMe == 3)
            {
                result = 4;
            }
        }

        return result;
    }

    /// <summary>Compute getxs.</summary>
    public int getxs()
    {
        int result = 0;
        if (isWolf)
        {
            result = 3;
            if (cdir == 1)
            {
                result = -3;
            }
        }

        if (isMoto)
        {
            if (arrItemMounts[4].template.id == 485)
            {
                result = 4;
                if (cdir == 1)
                {
                    result = -4;
                }
            }
            else
            {
                result = 7;
                if (cdir == 1)
                {
                    result = -7;
                }
            }
        }

        if (isNewMount)
        {
            result = 1;
            if (cdir == 1)
            {
                result = -1;
            }
        }

        return result;
    }

    // Offline timing fix: the original client painted once per game tick, and paintCharWithSkill also advances the
    // animation (indexSkill, the effect frames i0..i2, the arrow). Unity paints at the screen rate (60-144 fps) while
    // the logic runs at ~24 ticks/s, so skills played 3-6 times too fast. Only the first paint after a logic tick
    // advances the animation; later paints in the same tick redraw that frame from the state saved before it.
    private int skillPaintTick = -1;
    private struct SkillPaintState
    {
        // cf: Cf; i0: I0; i1: I1; i2: I2.
        public int cf, indexSkill, i0, i1, i2, dx0, dx1, dx2, dy0, dy1, dy2;
        public EffectCharPaint eff0, eff1, eff2;
        public Arrow arr;
    }

    private SkillPaintState skillPaintBefore;
    private SkillPaint skillPaintStepped; // the animation and frame left by the last step, to spot a restart
    private int skillPaintSteppedIndex;
    /// <summary>Perform kick option.</summary>
    /// <param name="item">Item instance being inspected or transferred.</param>
    /// <param name="maxKick">Max kick.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; i: Loop/collection index.</remarks>
    public void kickOption(Item item, int maxKick)
    {
        int num = 0;
        if (item == null || item.options == null)
        {
            return;
        }

        for (int i = 0; i < item.options.size(); i++)
        {
            ItemOption itemOption = (ItemOption)item.options.elementAt(i);
            itemOption.active = 0;
            if (itemOption.optionTemplate.type == 2)
            {
                if (num < maxKick)
                {
                    itemOption.active = 1;
                    num++;
                }
            }
            else if (itemOption.optionTemplate.type == 3 && item.upgrade >= 4)
            {
                itemOption.active = 1;
            }
            else if (itemOption.optionTemplate.type == 4 && item.upgrade >= 8)
            {
                itemOption.active = 1;
            }
            else if (itemOption.optionTemplate.type == 5 && item.upgrade >= 12)
            {
                itemOption.active = 1;
            }
            else if (itemOption.optionTemplate.type == 6 && item.upgrade >= 14)
            {
                itemOption.active = 1;
            }
            else if (itemOption.optionTemplate.type == 7 && item.upgrade >= 16)
            {
                itemOption.active = 1;
            }
        }
    }

    /// <summary>Check whether lang applies to the supplied state.</summary>
    public bool isLang()
    {
        if (TileMap.mapID == 1 /*|| TileMap.mapID == 27*/ || TileMap.mapID == 72 || TileMap.mapID == 10 || TileMap.mapID == 17 || TileMap.mapID == 22 || TileMap.mapID == 32 || TileMap.mapID == 38 || TileMap.mapID == 43 || TileMap.mapID == 48)
        {
            return true;
        }

        return false;
    }

    /// <summary>Check whether team applies to the supplied state.</summary>
    /// <param name="c">Client character being inspected or controlled.</param>
    /// <remarks>Locals: i: Loop/collection index.</remarks>
    public bool isTeam(Char c)
    {
        for (int i = 0; i < GameScr.vParty.size(); i++)
        {
            Party party = (Party)GameScr.vParty.elementAt(i);
            if (c.charID == party.charId)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Return ct.</summary>
    public static int getCT()
    {
        if (pointChienTruong >= 4000)
        {
            return 4;
        }

        if (pointChienTruong >= 1500)
        {
            return 3;
        }

        if (pointChienTruong >= 600)
        {
            return 2;
        }

        if (pointChienTruong >= 200)
        {
            return 1;
        }

        return 0;
    }

    /// <summary>Check whether humanz applies to the supplied state.</summary>
    public bool isHumanz()
    {
        return isHuman;
    }

    /// <summary>Check whether nhanbanz applies to the supplied state.</summary>
    public bool isNhanbanz()
    {
        return isNhanban;
    }
}
