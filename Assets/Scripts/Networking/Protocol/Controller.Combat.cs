using Functions;
using Functions.Auto;
using System;
using UnityEngine;

public partial class Controller
{
    /// <summary>Create skill.</summary>
    /// <param name="d">Distance or intermediate difference.</param>
    /// <remarks>Locals: i: Loop/collection index; j: Nested loop/collection index; k: Key or nested iteration index; l: Length or current list; m: Current message or map entry.</remarks>
    private void createSkill(myReader d)
    {
        GameScr.vcSkill = d.readByte();
        GameScr.sOptionTemplates = new SkillOptionTemplate[d.readByte()];
        for (int i = 0; i < GameScr.sOptionTemplates.Length; i++)
        {
            GameScr.sOptionTemplates[i] = new SkillOptionTemplate();
            GameScr.sOptionTemplates[i].id = i;
            GameScr.sOptionTemplates[i].name = d.readUTF();
        }

        GameScr.nClasss = new NClass[d.readUnsignedByte()];
        for (int j = 0; j < GameScr.nClasss.Length; j++)
        {
            GameScr.nClasss[j] = new NClass();
            GameScr.nClasss[j].classId = j;
            GameScr.nClasss[j].name = d.readUTF();
            GameScr.nClasss[j].skillTemplates = new SkillTemplate[d.readByte()];
            for (int k = 0; k < GameScr.nClasss[j].skillTemplates.Length; k++)
            {
                GameScr.nClasss[j].skillTemplates[k] = new SkillTemplate();
                GameScr.nClasss[j].skillTemplates[k].id = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].name = d.readUTF();
                GameScr.nClasss[j].skillTemplates[k].maxPoint = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].type = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].iconId = d.readShort();
                int lineWidth = 150;
                if (GameCanvas.w == 128 || GameCanvas.h <= 208)
                {
                    lineWidth = 100;
                }

                GameScr.nClasss[j].skillTemplates[k].description = mFont.tahoma_7_white.splitFontArray(d.readUTF(), lineWidth);
                GameScr.nClasss[j].skillTemplates[k].skills = new Skill[d.readByte()];
                for (int l = 0; l < GameScr.nClasss[j].skillTemplates[k].skills.Length; l++)
                {
                    GameScr.nClasss[j].skillTemplates[k].skills[l] = new Skill();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].skillId = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].template = GameScr.nClasss[j].skillTemplates[k];
                    GameScr.nClasss[j].skillTemplates[k].skills[l].point = d.readByte();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].level = d.readByte();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].manaUse = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].coolDown = d.readInt();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].dx = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].dy = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].maxFight = d.readByte();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].options = new SkillOption[d.readByte()];
                    for (int m = 0; m < GameScr.nClasss[j].skillTemplates[k].skills[l].options.Length; m++)
                    {
                        GameScr.nClasss[j].skillTemplates[k].skills[l].options[m] = new SkillOption();
                        GameScr.nClasss[j].skillTemplates[k].skills[l].options[m].param = d.readShort();
                        GameScr.nClasss[j].skillTemplates[k].skills[l].options[m].optionTemplate = GameScr.sOptionTemplates[d.readByte()];
                    }

                    Skills.add(GameScr.nClasss[j].skillTemplates[k].skills[l]);
                }
            }
        }

        AddBasicSkillGrowth();
    }

    // Offline deviation: extra rows (points 1..MaxPoint) for the basic attack, see BasicSkillGrowth.
    /// <summary>Add basic skill growth.</summary>
    /// <remarks>Locals: b: Entry selected from the source collection; p: Current player/point or request payload; s: Source text or serialized input; tpl: Static template record.</remarks>
    private static void AddBasicSkillGrowth()
    {
        if (GameScr.nClasss == null || GameScr.nClasss.Length == 0)
        {
            return;
        }

        foreach (SkillTemplate tpl in GameScr.nClasss[0].skillTemplates)
        {
            if (tpl.id != BasicSkillGrowth.TemplateId || tpl.skills == null || tpl.skills.Length != 1)
            {
                continue; // not the basic attack, or the rows were already added
            }

            Skill b = tpl.skills[0];
            Skill[] all = new Skill[BasicSkillGrowth.MaxPoint + 1];
            all[0] = b;
            for (int p = 1; p <= BasicSkillGrowth.MaxPoint; p++)
            {
                Skill s = new Skill();
                s.skillId = BasicSkillGrowth.SkillId(b.skillId, p);
                s.template = tpl;
                s.point = p;
                s.level = b.level;
                s.manaUse = b.manaUse;
                s.coolDown = BasicSkillGrowth.Cooldown(b.coolDown, p);
                s.dx = b.dx;
                s.dy = b.dy;
                s.maxFight = b.maxFight;
                s.options = b.options;
                all[p] = s;
                Skills.add(s);
            }

            tpl.skills = all;
        }
    }

    /// <summary>Add effect auto.</summary>
    /// <param name="msg">Framed protocol message.</param>
    /// <remarks>Locals: id: Identifier; x: Field decoded from the incoming binary payload; y: Field decoded from the incoming binary payload.</remarks>
    public void addEffAuto(Message msg)
    {
        try
        {
            short id = msg.reader().readUnsignedByte();
            int x = msg.reader().readShort();
            int y = msg.reader().readShort();
            sbyte loopCount = msg.reader().readByte();
            short time = msg.reader().readShort();
            EffectAuto.addEffectAuto(id, x, y, loopCount, time, 1);
        }
        catch (Exception)
        {
            mSystem.println("err add effAuto");
        }
    }

    /// <summary>Return data effect auto.</summary>
    /// <param name="msg">Framed protocol message.</param>
    /// <remarks>Locals: id: Identifier; num: Field decoded from the incoming binary payload.</remarks>
    public void getDataEffAuto(Message msg)
    {
        try
        {
            short id = msg.reader().readUnsignedByte();
            short num = msg.reader().readShort();
            sbyte[] data = null;
            if (num > 0)
            {
                data = new sbyte[num];
                msg.reader().read(ref data);
            }

            EffectAuto.reciveData(id, data);
        }
        catch (Exception)
        {
            mSystem.println("err add effAuto");
        }
    }

    /// <summary>Return image effect auto.</summary>
    /// <param name="msg">Framed protocol message.</param>
    /// <remarks>Locals: id: Identifier.</remarks>
    public void getImgEffAuto(Message msg)
    {
        try
        {
            short id = msg.reader().readUnsignedByte();
            sbyte[] data = NinjaUtil.readByteArray_Int(msg);
            EffectAuto.reciveImage(id, data);
        }
        catch (Exception)
        {
            mSystem.println("err getImgEffAuto");
        }
    }

    /// <summary>Add effect.</summary>
    /// <param name="msg">Framed protocol message.</param>
    /// <remarks>Locals: b: Field decoded from the incoming binary payload; iD: Identifier; num: Field decoded from the incoming binary payload; id: Field decoded from the incoming binary payload; num2: Field decoded from the incoming binary payload; b2: Field decoded from the incoming binary payload; b3: Field decoded from the incoming binary payload.</remarks>
    public void addEffect(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            MainObject mainObject;
            if (b == 1)
            {
                int iD = msg.reader().readUnsignedByte();
                mainObject = Mob.get_Mob(iD);
            }
            else
            {
                int num = msg.reader().readInt();
                mainObject = ((num != Char.getMyChar().charID) ? GameScr.findCharInMap(num) : Char.getMyChar());
            }

            if (mainObject != null)
            {
                short id = msg.reader().readShort();
                int num2 = msg.reader().readInt();
                sbyte b2 = msg.reader().readByte();
                sbyte b3 = msg.reader().readByte();
                bool isHead = ((b3 != 0) ? true : false);
                mainObject.addDataeff(id, num2, b2 * 1000, isHead);
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>Return image effect.</summary>
    /// <param name="msg">Framed protocol message.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; array: Field decoded from the incoming binary payload.</remarks>
    public void getImgEffect(Message msg)
    {
        try
        {
            short num = msg.reader().readUnsignedByte();
            sbyte[] array = NinjaUtil.readByteArray_Int(msg);
            GameData.setImgIcon(num, array);
            ImageIcon imageIcon = (ImageIcon)GameData.listImgIcon.get(string.Empty + num);
            if (imageIcon == null)
            {
                imageIcon = new ImageIcon();
                GameData.listImgIcon.put(num + string.Empty, imageIcon);
            }

            imageIcon.isLoad = false;
            imageIcon.img = createImage(array);
            if (GameMidlet.CLIENT_TYPE != 1)
            {
                RMS.saveRMSImage("ImgEffect " + num, array);
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>Return data effect.</summary>
    /// <param name="msg">Framed protocol message.</param>
    /// <remarks>Locals: num: Numeric intermediate used by this operation; the legacy name is retained; num2: Field decoded from the incoming binary payload.</remarks>
    public void getDataEffect(Message msg)
    {
        try
        {
            short num = msg.reader().readUnsignedByte();
            short num2 = msg.reader().readShort();
            sbyte[] data = null;
            if (num2 > 0)
            {
                data = new sbyte[num2];
                msg.reader().read(ref data);
            }

            ((EffectData)GameData.listbyteData.get(string.Empty + num))?.setdata(data);
        }
        catch (Exception)
        {
        }
    }
}
