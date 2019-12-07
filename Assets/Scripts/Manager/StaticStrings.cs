using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2 {
    public static class StaticStrings {
        #region INPUTS
        public static string Vertical = "Vertical";
        public static string Horizontal = "Horizontal";
        public static string B = "B";
        public static string A = "A";
        public static string X = "X";
        public static string Y = "Y";
        public static string RT = "RT";
        public static string LT = "LT";
        public static string RB = "RB";
        public static string LB = "LB";
        public static string R = "R";
        public static string L = "L";
        public static string Pad_x = "Pad_X";
        public static string Pad_y = "Pad_Y";
        public static string select = "Select";
        public static string start = "Start";
        public static string mouseX = "Mouse X";
        public static string mouseY = "Mouse Y";
        public static string rightAxisX = "RightAxis X";
        public static string rightAxisY = "RightAxis Y";
        #endregion
        #region ANIMATOR PARAMETERS
        public static string vertical = "vertical";
        public static string horizontal = "horizontal";
        public static string mirror = "mirror";
        public static string parry_attack = "parry_attack";
        public static string animSpeed = "animSpeed";
        public static string onGround = "onGround";
        public static string run = "run";
        public static string two_handed = "two_handed";
        public static string interacting = "interacting";
        public static string isLeft = "ifLeft";
        public static string blocking = "blocking";
        public static string canMove = "canMove";
        public static string onEmpty = "onEmpty";
        public static string lockon = "lockon";
        public static string spellcasting = "spellcasting";
        public static string enableItem = "enableItem";

        #endregion
        #region ANIMATOR STATES
        public static string Jump_start = "jump_launch";
        public static string Jump_land = "jump_land";
        public static string Rolls = "Rolls";
        public static string attack_interrupt = "attack_interrupt";
        public static string parried = "parried";
        public static string backstabbed = "backstabbed";
        public static string damage1 = "damage_1";
        public static string damage2 = "damage_2";
        public static string damage3 = "damage_3";
        public static string changeWeapon = "changeWeapon";
        public static string emptyBoth = "Empty Both";
        public static string emptyRight = "Empty Right";
        public static string emptyLeft = "Empty Left";
        public static string equipWeapon_oh = "equipWeapon_oh";
        public static string pick_up = "pick_up";
        #endregion
        #region UI
        public static string ui_ac_pick {
            get { return "Pick up item :" + A; }
        }
        public static string ui_ac_talk {
            get { return "Talk:" + A; }
        }
        public static string ui_ac_open {
            get { return "Open :" + A; }
        }
        public static string ui_ac_interact {
            get { return "Interact :" + A; }
        }
        #endregion
        #region OTHER
        public static string _l = "_l";
        public static string _r = "_r";
        #endregion
    }
}