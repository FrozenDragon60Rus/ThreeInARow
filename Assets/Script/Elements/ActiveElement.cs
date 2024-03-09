﻿using Assets.Script.Cells;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Elements
{
    internal class ActiveElement : Element
    {
        public override bool OnPosition
        {
            get
            {
                if (parent == null || this == null) return false;
                return transform.position.x == parent.position.x &&
                       transform.position.y == parent.position.y;
            }
        }

        public override Cell Parent
        {
            set
            {
                parent = value;
                StartCoroutine(Move());
            }
            get => parent;
        }

        public IEnumerator Move()
        {
            float speed = 0.5f * Time.fixedDeltaTime;
            while (true)
            {
                transform.position = Vector2.MoveTowards(transform.position, parent.position, speed);
                if (OnPosition) break;

                yield return false;
            }
            yield return true;
        }

        protected override void Reaction()
        {

        }
    }
}