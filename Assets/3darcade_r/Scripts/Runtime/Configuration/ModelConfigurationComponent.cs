/* MIT License

 * Copyright (c) 2020 Skurdt
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE. */

using UnityEngine;

namespace Arcade_r
{
    [DisallowMultipleComponent, SelectionBase]
    public sealed class ModelConfigurationComponent : MonoBehaviour
    {
        public string DescriptiveName                = default;
        public string Id                             = default;
        public string Model                          = default;
        public ContentInteraction ContentInteraction = default;
        public string ContentList                    = default;
        public bool MoveCabMovable                   = default;
        public bool MoveCabGrabbable                 = default;
        public Trigger[] Triggers                    = default;
        public string[] TriggerIDs                   = default;

        public void FromModelConfiguration(ModelConfiguration modelConfiguration)
        {
            DescriptiveName      = modelConfiguration.DescriptiveName;
            Id                   = modelConfiguration.Id;
            Model                = modelConfiguration.Model;
            ContentInteraction   = modelConfiguration.ContentInteraction;
            ContentList          = modelConfiguration.ContentList;
            MoveCabMovable       = modelConfiguration.MoveCabMovable;
            MoveCabGrabbable     = modelConfiguration.MoveCabGrabbable;
            Triggers             = modelConfiguration.Triggers;
            TriggerIDs           = modelConfiguration.TriggerIDs;
        }

        public ModelConfiguration ToModelConfiguration()
        {
            return new ModelConfiguration
            {
                DescriptiveName      = DescriptiveName,
                Id                   = Id,
                Model                = Model,
                ContentInteraction   = ContentInteraction,
                ContentList          = ContentList,
                MoveCabMovable       = MoveCabMovable,
                MoveCabGrabbable     = MoveCabGrabbable,
                Triggers             = Triggers,
                TriggerIDs           = TriggerIDs,

                Position = transform.localPosition,
                Rotation = MathUtils.CorrectEulerAngles(transform.localEulerAngles),
                Scale    = transform.localScale
            };
        }
    }
}
