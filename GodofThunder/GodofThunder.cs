namespace GodofThunder
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    using Ensage;
    using Ensage.Common;
    using Ensage.Common.Extensions;

    using SharpDX;
    using SharpDX.Direct3D9;

    using unitDB = Ensage.Common.UnitDatabase;

    internal class GodofThunder
    {
        //item
        private static Item scytheOfVyse;
        private static Item blink;
        private static float blinkRange;
        private static Item orchid;
        private static Item arcane;
        private static Item shiva;
        private static Item dagon;
        private static Item veil;
        private static Item soulring;

        //skills
        private static Ability arcQ;
        private static bool enableQ;
        private static double arcQcastpoint;

        private static Ability lightningW;
        private static double lightningWcastpoint;

        private static Ability WrathR;
        private static bool enableR;
        private static double WrathRcastpoint;

        //things i don't know lmao

        private static bool loaded;
        private static Hero me;
        private static Hero target;
        private static float targetDistance;
        private static Vector3 mePosition;
        private static float hullsum;
        private static double turnTime;
        public static Font text;

        public static void Init()
        {
            Game.OnUpdate += Game_OnUpdate;
            loaded = false;
            me = null;
            target = null;

            //item
            scytheOfVyse = null;
            blink = null;
            arcane = null;
            orchid = null;
            dagon = null;
            veil = null;
            soulring = null;
            shiva = null;
            text = new Font(
                Drawing.Direct3DDevice9,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = 13,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default
                });

            Drawing.OnPreReset += Drawing_OnPreReset;
            Drawing.OnPostReset += Drawing_OnPostReset;
            Drawing.OnEndScene += Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainDomainUnload;
            Game.OnWndProc += Game_OnWndProc;
            Orbwalking.Load();

        }

        private static bool CastCombo()
        {
            var canCancel = (Orbwalking.CanCancelAnimation()
                             && (Orbwalking.AttackOnCooldown(target) || me.NetworkActivity != (NetworkActivity)1503))
                            || (!Orbwalking.AttackOnCooldown(target) && targetDistance > 250);
            if (!Utils.SleepCheck("casting") || !me.CanCast() || !target.IsVisible || !canCancel)
            {
                return false;
            }

            //HEX
            if (scytheOfVyse != null && scytheOfVyse.CanBeCasted() &&
                targetDistance <= (scytheOfVyse.CastRange + hullsum) && Utils.SleepCheck("hex"))
            {
                var canUse = Utils.ChainStun(target, turnTime + 0.1 + Game.Ping / 1000, null, false);
                if (canUse)
                {
                    scytheOfVyse.UseAbility(target);
                    Utils.Sleep(turnTime * 1000 + 100 + Game.Ping, "hex");
                    Utils.Sleep(turnTime * 1000 + 50, "move");
                    Utils.Sleep(
                        turnTime * 1000 + 100
                        + (Math.Max(targetDistance - hullsum - scytheOfVyse.CastRange, 1000) / me.MovementSpeed) * 1000,
                        "casting");
                    Utils.Sleep(turnTime * 1000 + 200, "CHAINSTUN_SLEEP");
                    return true;
                }
            }
            //Veil of Discord
            if (veil != null && veil.CanBeCasted() &&
                targetDistance <= (veil.CastRange + hullsum) && Utils.SleepCheck("veil"))
            {
                var canUse = Utils.ChainStun(target, turnTime + 0.1 + Game.Ping / 1000, null, false);
                if (canUse)
                {
                    veil.UseAbility(target.Position);
                    Utils.Sleep(turnTime * 1000 + 100 + Game.Ping, "veil");
                    Utils.Sleep(turnTime * 1000 + 50, "move");
                    Utils.Sleep(
                        turnTime * 1000 + 100
                        + (Math.Max(targetDistance - hullsum - veil.CastRange, 1000) / me.MovementSpeed) * 1000,
                        "casting");
                    Utils.Sleep(turnTime * 1000 + 200, "veil");
                    return true;
                }
            }

            //Orchid
            if (orchid != null && orchid.CanBeCasted() &&
                targetDistance <= (orchid.CastRange + hullsum) && Utils.SleepCheck("orchid"))
            {
                var canUse = Utils.ChainStun(target, turnTime + 0.1 + Game.Ping / 1000, null, false);
                if (canUse)
                {
                    orchid.UseAbility(target);
                    Utils.Sleep(turnTime * 1000 + 100 + Game.Ping, "orchid");
                    Utils.Sleep(turnTime * 1000 + 50, "move");
                    Utils.Sleep(
                        turnTime * 1000 + 100
                        + (Math.Max(targetDistance - hullsum - orchid.CastRange, 1000) / me.MovementSpeed) * 1000,
                        "casting");
                    Utils.Sleep(turnTime * 1000 + 200, "CHAINSTUN_SLEEP");
                    return true;
                }
            }

            //Shiva

            if (shiva != null && shiva.CanBeCasted() &&
                targetDistance <= (shiva.CastRange + hullsum) && Utils.SleepCheck("shiva"))
            {
                var canUse = Utils.ChainStun(target, turnTime + 0.1 + Game.Ping / 1000, null, false);
                if (canUse)
                {
                    shiva.UseAbility();
                    Utils.Sleep(turnTime * 1000 + 100 + Game.Ping, "shiva");
                    Utils.Sleep(turnTime * 1000 + 50, "move");
                    Utils.Sleep(
                        turnTime * 1000 + 100
                        + (Math.Max(targetDistance - hullsum - shiva.CastRange, 1000) / me.MovementSpeed) * 1000,
                        "casting");
                    Utils.Sleep(turnTime * 1000 + 200, "shiva");
                    return true;
                }
            }


            //Dagon

            if (dagon != null && dagon.CanBeCasted() &&
                targetDistance <= (dagon.CastRange + hullsum) && Utils.SleepCheck("dagon"))
            {
                var canUse = Utils.ChainStun(target, turnTime + 0.1 + Game.Ping / 1000, null, false);
                if (canUse)
                {
                    dagon.UseAbility(target);
                    Utils.Sleep(turnTime * 1000 + 100 + Game.Ping, "dagon");
                    Utils.Sleep(turnTime * 1000 + 50, "move");
                    Utils.Sleep(
                        turnTime * 1000 + 100
                        + (Math.Max(targetDistance - hullsum - dagon.CastRange, 800) / me.MovementSpeed) * 1000,
                        "casting");
                    Utils.Sleep(turnTime * 1000 + 200, "dagon");
                    return true;
                }
            }

            if (me.Health > 300 && me.Mana < me.Spellbook.Spell2.ManaCost && soulring.CanBeCasted())
            {
                soulring.UseAbility();
                Utils.Sleep(turnTime * 1000 + 100 + Game.Ping, "soulring");
            }

            if (me.Mana < me.Spellbook.Spell2.ManaCost && arcane.CanBeCasted())
            {
                arcane.UseAbility();
                Utils.Sleep(turnTime * 1000 + 100 + Game.Ping, "arcane");
            }

            // Skills
            if (arcQ.CanBeCasted() && Utils.SleepCheck("Q") && enableQ
                && ((me.Mana - arcQ.ManaCost) > lightningW.ManaCost || !lightningW.CanBeCasted()))
            {
                var radius = arcQ.AbilityData.FirstOrDefault(x => x.Name == "arc").GetValue(0);
                var pos = target.Position
                          + target.Vector3FromPolarAngle() * ((Game.Ping / 1000 + 0.3f) * target.MovementSpeed);

                if (mePosition.Distance(pos) < targetDistance)
                {
                    pos = target.Position;
                }
                if (mePosition.Distance2D(pos) <= (radius)
                    && (orchid == null || !orchid.CanBeCasted() || mePosition.Distance2D(pos) > 200)
                    && (scytheOfVyse == null || !scytheOfVyse.CanBeCasted() || mePosition.Distance2D(pos) > 200)
                    && (veil == null || !veil.CanBeCasted() || mePosition.Distance2D(pos) > 200)
                    && (dagon == null || !dagon.CanBeCasted() || mePosition.Distance2D(pos) > 200))
                {
                    var canUse = Utils.ChainStun(target, 0.3 + Game.Ping / 1000, null, false);
                    if (canUse)
                    {
                        arcQ.UseAbility(target);
                        Utils.Sleep(arcQcastpoint * 1000 + Game.Ping, "Q");
                        Utils.Sleep(arcQcastpoint * 1000, "casting");
                        return true;
                    }
                }
                else if (Utils.SleepCheck("moveCloser"))
                {
                    me.Move(pos);
                    Utils.Sleep(200, "moveCloser");
                    return true;
                }
            }

            //********************************************

            if (blink != null && blink.CanBeCasted() && targetDistance > 400
               && targetDistance < (blinkRange + hullsum * 2 + me.AttackRange) && Utils.SleepCheck("blink"))
            {
                var position = target.Position;
                if (target.NetworkActivity != (NetworkActivity)1500)
                {
                    position = target.Position + target.Vector3FromPolarAngle() * (hullsum + me.AttackRange);
                    if (mePosition.Distance(position) < targetDistance)
                    {
                        position = target.Position;
                    }
                }
                var dist = position.Distance2D(mePosition);
                if (dist > blinkRange)
                {
                    position = (position - mePosition) * (blinkRange - 1) / position.Distance2D(me) + mePosition;
                }
                blink.UseAbility(position);
                mePosition = position;
                Utils.Sleep(turnTime * 1000 + 100 + Game.Ping, "blink");
                Utils.Sleep(turnTime * 1000 + 50, "move");
                Utils.Sleep(turnTime * 1000, "casting");
                return true;
            }
            const int Radius = 300;
            var canAttack = !target.IsInvul() && !target.IsAttackImmune() && me.CanAttack();
            if (!canAttack)
            {
                return false;
            }
            if (arcQ.CanBeCasted() && Utils.SleepCheck("W")
                && !(arcQ.CanBeCasted() && enableQ && Utils.ChainStun(target, 0.3 + Game.Ping / 1000, null, false)))
            {
                if (mePosition.Distance2D(target) <= (Radius + hullsum))
                {
                    lightningW.UseAbility(target);
                    Utils.Sleep(lightningWcastpoint * 1000 + Game.Ping, "W");
                    Utils.Sleep(lightningWcastpoint * 1000, "casting");
                    return true;
                }
            }
            if (!WrathR.CanBeCasted() || !Utils.SleepCheck("R"))
            {
                return false;
            }
            if (!(mePosition.Distance2D(target) <= (Radius + hullsum)))
            {
                return false;
            }
            WrathR.UseAbility();
            Utils.Sleep(100 + Game.Ping, "R");
            Utils.Sleep(100, "casting");
            return true;
        }

        private static void CurrentDomainDomainUnload(object sender, EventArgs e)
        {
            text.Dispose();
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice9 == null || Drawing.Direct3DDevice9.IsDisposed || !Game.IsInGame)
            {
                return;
            }

            var player = ObjectMgr.LocalPlayer;
            if (player == null || player.Team == Team.Observer)
            {
                return;
            }

            text.DrawText(
                null,
                enableQ ? "ThunderGod Arc: Q - ENABLED! | [G] for toggle" : "ThunderGod Wrath: Q - DISABLED! | [G] for toggle",
                5,
                96,
                Color.IndianRed);
        }

        private static void Drawing_OnPostReset(EventArgs args)
        {
            text.OnResetDevice();
        }

        private static void Drawing_OnPreReset(EventArgs args)
        {
            text.OnLostDevice();
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!loaded)
            {
                me = ObjectMgr.LocalHero;
                if (!Game.IsInGame || me == null || me.ClassID != ClassID.CDOTA_Unit_Hero_Ursa)
                {
                    return;
                }
                arcQ = me.Spellbook.Spell1;
                lightningW = me.Spellbook.SpellW;
                WrathR = me.FindSpell("zuus_thundergods_wrath");
                blink = me.FindItem("item_blink");
                orchid = me.FindItem("item_orchid");
                scytheOfVyse = me.FindItem("item_sheepstick");
                arcane = me.FindItem("item_arcane_boots");
                shiva = me.FindItem("item_shivas_guard");
                soulring = me.FindItem("item_soul_ring");
                dagon = me.FindItem("item_dagon");
                veil = me.FindItem("item_veil_of_discord");
                loaded = true;
            }

            if (!Game.IsInGame || me == null)
            {
                lightningWcastpoint = 1000;
                arcQcastpoint = 1000;
                loaded = false;
                me = null;
                target = null;
                arcQ = null;
                lightningW = null;
                WrathR = null;
                dagon = null;
                scytheOfVyse = null;
                blink = null;
                arcane = null;
                shiva = null;
                soulring = null;
                orchid = null;
                veil = null;

                return;
            }

            if (Game.IsPaused)
            {
                return;
            }

            if (blink == null)
            {
                blink = me.FindItem("item_blink");
            }

            if (orchid == null)
            {
                orchid = me.FindItem("item_orchid");
            }

            if (shiva == null)
            {
                shiva = me.FindItem("item_shivas_guard");
            }

            if (dagon == null)
            {
                dagon = me.FindItem("item_dagon");
            }

            if (scytheOfVyse == null)
            {
                scytheOfVyse = me.FindItem("item_sheepstick");
            }

            if (veil == null)
            {
                veil = me.FindItem("item_veil_of_discord");
            }

            if (soulring == null)
            {
                soulring = me.FindItem("item_soul_ring");
            }

            if (arcQ == null)
            {
                arcQ = me.Spellbook.Spell1;
            }
            else if (arcQcastpoint == 1000)
            {
                arcQcastpoint = 0.3;
            }

            if (lightningW == null)
            {
                lightningW = me.Spellbook.SpellW;
            }
            else if (lightningWcastpoint == 1000)
            {
                lightningWcastpoint = 0.3;
            }

            if (WrathR == null)
            {
                WrathR = me.FindSpell("zuus_thundergods_wrath");
            }

            if (!Game.IsKeyDown(Key.Space) || Game.IsChatOpen)
            {
                target = null;
                return;
            }
            if (Utils.SleepCheck("blink"))
            {
                mePosition = me.Position;
            }
            if (arcQ.IsInAbilityPhase && (target == null || !target.IsAlive || target.Distance2D(me) > arcQ.AbilityData.FirstOrDefault(x => x.Name == "arcQ").GetValue(0)))
            {
                me.Stop();
                if (target != null)
                {
                    me.Attack(target);
                }
            }
            if (lightningW.IsInAbilityPhase && (target == null || !target.IsAlive))
            {
                me.Stop();
                if (target != null)
                {
                    me.Attack(target);
                }
            }
            var range = 1000f;
            var mousePosition = Game.MousePosition;
            if (blink != null)
            {
                blinkRange = blink.AbilityData.FirstOrDefault(x => x.Name == "blink_range").GetValue(0);
                range = blinkRange + me.HullRadius + 500;
            }
            var canCancel = (Orbwalking.CanCancelAnimation() && Orbwalking.AttackOnCooldown(target))
                            || (!Orbwalking.AttackOnCooldown(target)
                                && (targetDistance > 350 || (target != null && !target.IsVisible))) || target == null;
            if (canCancel)
            {
                if (target != null && !target.IsVisible)
                {
                    var closestToMouse = me.ClosestToMouseTarget(128);
                    if (closestToMouse != null)
                    {
                        target = me.ClosestToMouseTarget(range);
                    }
                }
                else
                {
                    target = me.ClosestToMouseTarget(range);
                }
            }
            if (target == null || !target.IsAlive || ((!target.IsVisible
                   || target.Distance2D(mousePosition) > target.Distance2D(me) + 1000) && canCancel))
            {
                if (!Utils.SleepCheck("move"))
                {
                    return;
                }
                me.Move(mousePosition);
                Utils.Sleep(100, "move");
                return;
            }
            targetDistance = mePosition.Distance2D(target);
            hullsum = (me.HullRadius + target.HullRadius) * 2;
            turnTime = me.GetTurnTime(target);
            var casting = CastCombo();
            if (casting)
            {
                return;
            }
            if (!Utils.SleepCheck("casting"))
            {
                return;
            }
            OrbWalk(Orbwalking.CanCancelAnimation());
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != (ulong)Utils.WindowsMessages.WM_KEYUP || args.WParam != 'G' || Game.IsChatOpen)
            {
                return;
            }
            enableQ = !enableQ;
        }

        private static void OrbWalk(bool canCancel)
        {
            //var modifier = target.Modifiers.FirstOrDefault(x => x.Name == "modifier_ursa_fury_swipes_damage_increase");
            // && UnitDatabase.GetAttackSpeed(me) < 300 && !me.Modifiers.Any(x => x.Name == "modifier_ursa_overpower")
            //var overpowering = me.Modifiers.Any(x => x.Name == "modifier_ursa_overpower");
            var canAttack = !Orbwalking.AttackOnCooldown(target) && !target.IsInvul() && !target.IsAttackImmune()
                            && me.CanAttack();
            if (canAttack && (targetDistance <= (350)))
            {
                if (!Utils.SleepCheck("attack"))
                {
                    return;
                }
                me.Attack(target);
                Utils.Sleep(100, "attack");
                return;
            }

            var canMove = (canCancel && Orbwalking.AttackOnCooldown(target)) || (!Orbwalking.AttackOnCooldown(target) && targetDistance > 350);
            if (!Utils.SleepCheck("move") || !canMove)
            {
                return;
            }
            var mousePos = Game.MousePosition;
            if (target.Distance2D(me) < 500)
            {
                var pos = target.Position
                          + target.Vector3FromPolarAngle()
                          * (float)Math.Max((Game.Ping / 1000 + (targetDistance / me.MovementSpeed) + turnTime) * target.MovementSpeed, 500);

                //Console.WriteLine(pos.Distance(me.Position) + " " + target.Distance2D(pos));
                if (pos.Distance(me.Position) > target.Distance2D(pos) - 80)
                {

                    me.Move(pos);
                }
                else
                {
                    me.Follow(target);
                }
            }
            else
            {
                me.Move(mousePos);
            }
            Utils.Sleep(100, "move");
        }

    }
}
