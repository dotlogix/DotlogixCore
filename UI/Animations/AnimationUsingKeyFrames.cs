// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AnimationUsingKeyFrames.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;
#endregion

namespace DotLogix.UI.Animations {
    /// <summary>
    ///     Generic animation class for TValue type. Animates along key frames set. Uses TAnimationHelper to animate value. For
    ///     using in XAML subclass it with non-generic class.
    /// </summary>
    [ContentProperty("KeyFrames")]
    public class AnimationUsingKeyFrames<TValue, TAnimationHelper, TKeyFrameCollection> :
    AnimationBase<TValue>, IKeyFrameAnimation, IAddChild
        where TAnimationHelper : class, IAnimationHelper<TValue>, new()
        where TKeyFrameCollection : Freezable, IList<KeyFrame<TValue>>, IList, new() {
        #region Constructors
        public AnimationUsingKeyFrames() {
            _keyFramesResolved = true;
        }
        #endregion

        #region IKeyFrameAnimation
        IList IKeyFrameAnimation.KeyFrames {
            get { return KeyFrames; }
            set { KeyFrames = (TKeyFrameCollection)value; }
        }
        #endregion

        #region AnimationTimeline
        protected sealed override Duration GetNaturalDurationCore(Clock clock) {
            return new Duration(GetBiggestKeyFrameTime());
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Returns true if the value of the KeyFrames property of this AnimationUsingKeyFrames should be value-serialized
        /// </summary>
        public bool ShouldSerializeKeyFrames() {
            ReadPreamble();
            return (KeyFrameList != null) && (KeyFrameList.Count > 0);
        }
        #endregion

        #region IAddChild
        void IAddChild.AddChild(object child) {
            if(child == null)
                throw new ArgumentNullException("child");
            WritePreamble();
            AddChild(child);
            WritePostscript();
        }

        void IAddChild.AddText(string childText) {
            if(childText == null)
                throw new ArgumentNullException("childText");
            AddText(childText);
        }

        /// <summary>
        ///     Allows KeyFrames to be direct children of Animations in XAML.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void AddChild(object child) {
            var keyFrame = child as KeyFrame<TValue>;
            if(keyFrame == null)
                throw new ArgumentException("Child should be of type KeyFrame<" + typeof(TValue).Name + ">");
            KeyFrames.Add(keyFrame);
        }

        /// <summary>
        ///     Animation supports only KeyFrames as children.
        /// </summary>
        /// <exception cref="InvalidOperationException">throws always</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void AddText(string childText) {
            throw new InvalidOperationException("Animation doesn't support text children. Only KeyFrame<" +
                                                typeof(TValue).Name + "> is acceptable");
        }
        #endregion

        #region Freezable
        /// <summary>
        ///     Clones this AnimationUsingKeyFrame.
        /// </summary>
        public new AnimationUsingKeyFrames<TValue, TAnimationHelper, TKeyFrameCollection> Clone() {
            return (AnimationUsingKeyFrames<TValue, TAnimationHelper, TKeyFrameCollection>)base.Clone();
        }

        /// <summary>
        ///     Makes copy of the class with current state of values. Since class is not animated method similar to Clone()
        /// </summary>
        public new AnimationUsingKeyFrames<TValue, TAnimationHelper, TKeyFrameCollection> CloneCurrentValue() {
            return (AnimationUsingKeyFrames<TValue, TAnimationHelper, TKeyFrameCollection>)base.CloneCurrentValue();
        }

        protected override Freezable CreateInstanceCore() {
            return new AnimationUsingKeyFrames<TValue, TAnimationHelper, TKeyFrameCollection>();
        }

        protected override void CloneCore(Freezable sourceFreezable) {
            base.CloneCore(sourceFreezable);
            CopyKeyFrames(sourceFreezable);
        }

        protected override void CloneCurrentValueCore(Freezable sourceFreezable) {
            base.CloneCurrentValueCore(sourceFreezable);
            CopyKeyFrames(sourceFreezable, true);
        }

        protected override void GetAsFrozenCore(Freezable sourceFreezable) {
            base.GetAsFrozenCore(sourceFreezable);
            CopyKeyFrames(sourceFreezable, asFrozen: true);
        }

        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable) {
            base.GetCurrentValueAsFrozenCore(sourceFreezable);
            CopyKeyFrames(sourceFreezable, true, true);
        }

        /// <summary>
        ///     Makes this Timeline unmodifiable or determines whether it can be made unmodifiable.
        /// </summary>
        protected override bool FreezeCore(bool isChecking) {
            if(!base.FreezeCore(isChecking))
                return false;
            if(!Freeze(_keyFrames, isChecking))
                return false;
            if(!isChecking && !_keyFramesResolved)
                ResolveKeyFrames();
            return true;
        }

        protected override void OnChanged() {
            //function can be called when freezing and already frozen - prevent it
            if(!IsFrozen)
                _keyFramesResolved = false;
            base.OnChanged();
        }
        #endregion

        #region AnimationBase
        protected sealed override TValue GetCurrentValueCore(TValue defaultOriginValue, TValue defaultDestinationValue,
                                                             AnimationClock animationClock) {
            Debug.Assert(animationClock.CurrentState != ClockState.Stopped);

            if(_keyFrames == null)
                return defaultDestinationValue;
            // if resolving was reset while freezing - recalculate
            if(!_keyFramesResolved)
                ResolveKeyFrames();
            if(_keyFrameRefs == null)
                return defaultDestinationValue;

            var currTime = animationClock.CurrentTime.GetValueOrDefault();
            var keyFrameCount = _keyFrameRefs.Length;
            var currKeyFrameRefIdx = FindKeyFrameRefIdxForTime(currTime);

            TValue currValue;
            if(currKeyFrameRefIdx == keyFrameCount)
            // After the last key frame - take last frame value
                currValue = GetResolvedKeyFrame(keyFrameCount - 1).Value;
            else if(currTime == _keyFrameRefs[currKeyFrameRefIdx].ResolvedTime)
            // Exactly on a key frame. 
                currValue = GetResolvedKeyFrame(currKeyFrameRefIdx).Value;
            else if(currKeyFrameRefIdx == 0) {
                // before first frame - special case
                var baseValue = IsAdditive && AnimationHelper.IsAccumulable
                                    ? AnimationHelper.GetZeroValue()
                                    : defaultOriginValue;
                var progress = currTime.TotalMilliseconds / _keyFrameRefs[0].ResolvedTime.TotalMilliseconds;
                currValue = GetResolvedKeyFrame(currKeyFrameRefIdx).InterpolateValue(baseValue, progress);
            } else {
                // Between key frames.
                var baseValue = GetResolvedKeyFrame(currKeyFrameRefIdx - 1).Value;
                var prevKeyFrameTime = _keyFrameRefs[currKeyFrameRefIdx - 1].ResolvedTime;
                var segmentCurrTime = currTime - prevKeyFrameTime;
                var segmentDuration = _keyFrameRefs[currKeyFrameRefIdx].ResolvedTime - prevKeyFrameTime;
                var progress = segmentCurrTime.TotalMilliseconds / segmentDuration.TotalMilliseconds;
                currValue = GetResolvedKeyFrame(currKeyFrameRefIdx).InterpolateValue(baseValue, progress);
            }

            if(AnimationHelper.IsAccumulable) {
                if(IsCumulative) {
                    var currentRepeat = animationClock.CurrentIteration.GetValueOrDefault() - 1;
                    if(currentRepeat > 0) {
                        currValue =
                        AnimationHelper.AddValues(currValue,
                                                  AnimationHelper.
                                                  ScaleValue(GetResolvedKeyFrame(keyFrameCount - 1).Value,
                                                             currentRepeat));
                    }
                }
                if(IsAdditive)
                    return AnimationHelper.AddValues(defaultOriginValue, currValue);
            }
            return currValue;
        }

        private int FindKeyFrameRefIdxForTime(TimeSpan currTime) {
            var keyFrameCount = _keyFrameRefs.Length;
            var currKeyFrameRefIdx = 0;
            // Skip all frames before the time. 
            while((currKeyFrameRefIdx < keyFrameCount) && (currTime > _keyFrameRefs[currKeyFrameRefIdx].ResolvedTime))
                currKeyFrameRefIdx++;
            // select last one from multiple frames with same key time. 
            while((currKeyFrameRefIdx < (keyFrameCount - 1)) &&
                  (currTime == _keyFrameRefs[currKeyFrameRefIdx + 1].ResolvedTime))
                currKeyFrameRefIdx++;
            return currKeyFrameRefIdx;
        }
        #endregion

        #region Public Properties
        #region Dependency Properties
        /// <summary>
        ///     If this property is set to true base value will be incremented by the the current animation value instead of
        ///     replacing it entirely.
        /// </summary>
        public bool IsAdditive {
            get { return (bool)GetValue(IsAdditiveProperty); }
            set { SetValue(IsAdditiveProperty, value); }
        }

        /// <summary>
        ///     When this property is set to true, the animation's output values only accumulate when the animation's
        ///     RepeatBehavior property causes it to repeat its simple duration. It does not accumulate its values when it restarts
        ///     because its parent repeated or because its clock was restarted from a Begin call.
        /// </summary>
        /// <remarks>
        ///     This property works with the IsAdditive property and has no effect without it.
        /// </remarks>
        public bool IsCumulative {
            get { return (bool)GetValue(IsCumulativeProperty); }
            set { SetValue(IsCumulativeProperty, value); }
        }
        #endregion

        public TKeyFrameCollection KeyFrames {
            get {
                ReadPreamble();
                if(_keyFrames == null) {
                    if(IsFrozen)
                        _keyFrames = GetEmptyKeyFrames();
                    else
                        KeyFrames = new TKeyFrameCollection();
                }

                return _keyFrames;
            }
            set {
                if(value == null)
                    throw new ArgumentNullException("value");
                WritePreamble();
                if(_keyFrames == value)
                    return;
                OnFreezablePropertyChanged(_keyFrames, value);
                _keyFrames = value;
                WritePostscript();
            }
        }
        #endregion

        #region Private Methods
        private KeyFrame<TValue> GetResolvedKeyFrame(int resolvedKeyFrameIndex) {
            Debug.Assert(_keyFramesResolved, "Call ResolveKeyFrames() before");
            return KeyFrameList[_keyFrameRefs[resolvedKeyFrameIndex].OriginalIndex];
        }

        private TimeSpan GetBiggestKeyFrameTime() {
            if(_keyFrames == null)
                return TimeSpan.FromSeconds(1.0);
            var biggestTime = TimeSpan.Zero;
            foreach(var keyFrame in _keyFrames) {
                var keyTime = keyFrame.KeyTime;
                if((keyTime.Type == KeyTimeType.TimeSpan) && (keyTime.TimeSpan > biggestTime))
                    biggestTime = keyTime.TimeSpan;
            }
            return biggestTime != TimeSpan.Zero ? biggestTime : TimeSpan.FromSeconds(1.0);
        }

        private void ResolveKeyFrames() {
            if(_keyFramesResolved)
                return;
            if((KeyFrameList == null) || (KeyFrameList.Count == 0)) {
                _keyFrameRefs = null;
                _keyFramesResolved = true;
                return;
            }
            var keyFrameCount = KeyFrameList.Count;
            _keyFrameRefs = new KeyFrameRef[keyFrameCount];
            // Initialize the OriginalIndex.
            for(var idx = 0; idx < keyFrameCount; idx++)
                _keyFrameRefs[idx].OriginalIndex = idx;

            var uniformRangeList = new List<KeyFrameRange>();
            // Resolve Percent and Time key frames. Collect Uniform blocks, check if paced key frames exists
            var pacedFound = ResolveTimeAndPercentKeyFrames(uniformRangeList);
            ResolveUniformKeyFrames(uniformRangeList);
            if(pacedFound)
                ResolvePacedKeyFrames();
            Array.Sort(_keyFrameRefs); // Sort key frame refs by resolved times
            _keyFramesResolved = true;
        }

        private bool ResolveTimeAndPercentKeyFrames(ICollection<KeyFrameRange> uniformRangeList) {
            var duration = Duration;
            var calculatedDuration = duration.HasTimeSpan ? duration.TimeSpan : GetBiggestKeyFrameTime();
            var pacedFound = false;
            var keyFrameCount = KeyFrameList.Count;
            var idx = 0;
            while(idx < keyFrameCount) {
                var keyTime = KeyFrameList[idx].KeyTime;

                switch(keyTime.Type) {
                    case KeyTimeType.TimeSpan:
                        _keyFrameRefs[idx].ResolvedTime = keyTime.TimeSpan;
                        idx++;
                        break;
                    case KeyTimeType.Percent:
                        _keyFrameRefs[idx].ResolvedTime =
                        TimeSpan.FromMilliseconds(keyTime.Percent * calculatedDuration.TotalMilliseconds);
                        idx++;
                        break;
                    case KeyTimeType.Uniform:
                    case KeyTimeType.Paced:
                        if(idx == (keyFrameCount - 1)) {
                            _keyFrameRefs[idx].ResolvedTime = calculatedDuration;
                            idx++;
                        } else if((idx == 0) && (keyTime.Type == KeyTimeType.Paced)) {
                            // Note: this if should come after the previous if because of rule precedence. 
                            _keyFrameRefs[idx].ResolvedTime = TimeSpan.Zero;
                            idx++;
                        } else {
                            var range = new KeyFrameRange {FirstIndex = idx};
                            if(keyTime.Type == KeyTimeType.Paced)
                                pacedFound = true;
                            // skip until incompatible type
                            while(++idx < (keyFrameCount - 1)) {
                                var type = KeyFrameList[idx].KeyTime.Type;
                                if((type == KeyTimeType.Percent) || (type == KeyTimeType.TimeSpan))
                                    break;
                                if(type == KeyTimeType.Paced)
                                    pacedFound = true;
                            }
                            range.LastIndex = idx;
                            uniformRangeList.Add(range);
                        }
                        break;
                }
            }
            return pacedFound;
        }

        private void ResolveUniformKeyFrames(IEnumerable<KeyFrameRange> uniformRangeList) {
            foreach(var range in uniformRangeList) {
                // Segments = uniform key frame count + 1
                long segmentCount = (range.LastIndex - range.FirstIndex) + 1;
                var startTime = range.FirstIndex > 0 ? _keyFrameRefs[range.FirstIndex - 1].ResolvedTime : TimeSpan.Zero;
                var uniformTimeStep =
                TimeSpan.FromTicks((_keyFrameRefs[range.LastIndex].ResolvedTime - startTime).Ticks / segmentCount);

                var resolvedTime = startTime + uniformTimeStep;
                for(var idx = range.FirstIndex; idx < range.LastIndex; idx++) {
                    _keyFrameRefs[idx].ResolvedTime = resolvedTime;
                    resolvedTime += uniformTimeStep;
                }
            }
        }

        private void ResolvePacedKeyFrames() {
            if((KeyFrameList == null) || (KeyFrameList.Count <= 2))
                return;

            var idx = 1; // the first key frame should be already resolved start from second.
            var lastKeyFrameIdx = _keyFrameRefs.Length - 1;

            do {
                if(KeyFrameList[idx].KeyTime.Type != KeyTimeType.Paced) {
                    idx++;
                    continue;
                }

                var startIdx = idx;
                var segmentLengths = new List<double>();
                var startTime = _keyFrameRefs[idx - 1].ResolvedTime;
                var startValue = KeyFrameList[idx - 1].Value;
                var totalSegmentLength = 0.0;
                // calc segment length for all paced segments in block
                do {
                    var currValue = KeyFrameList[idx].Value;
                    totalSegmentLength += AnimationHelper.GetSegmentLength(startValue, currValue);
                    segmentLengths.Add(totalSegmentLength); // store for later use
                    startValue = currValue;
                    idx++;
                } while((idx < lastKeyFrameIdx) && (KeyFrameList[idx].KeyTime.Type == KeyTimeType.Paced));
                // calc segment length also for last segment (to the non-paced frame)
                totalSegmentLength += AnimationHelper.GetSegmentLength(startValue, KeyFrameList[idx].Value);

                // Calculate duration of all segments.
                var totalSegmentDuration = (_keyFrameRefs[idx].ResolvedTime - startTime).TotalMilliseconds;

                // Calc time for frames from segment lengths and total duration. 
                for(int i = 0, currIdx = startIdx; i < segmentLengths.Count; i++, currIdx++) {
                    _keyFrameRefs[currIdx].ResolvedTime =
                    startTime + TimeSpan.FromMilliseconds((segmentLengths[i] / totalSegmentLength) *
                                                          totalSegmentDuration);
                }
            } while(idx < lastKeyFrameIdx);
        }

        private void CopyKeyFrames(Freezable source, bool copyCurrent = false, bool asFrozen = false) {
            var sourceAnimation = (AnimationUsingKeyFrames<TValue, TAnimationHelper, TKeyFrameCollection>)source;
            //frozen will be reset anyway
            if(!asFrozen && !IsFrozen) {
                _keyFramesResolved = sourceAnimation._keyFramesResolved;

                if(_keyFramesResolved && (sourceAnimation._keyFrameRefs != null))
                    _keyFrameRefs = (KeyFrameRef[])sourceAnimation._keyFrameRefs.Clone();
            }

            if(sourceAnimation._keyFrames == null)
                return;
            _keyFrames = (TKeyFrameCollection)(copyCurrent
                                                   ? sourceAnimation._keyFrames.CloneCurrentValue()
                                                   : sourceAnimation._keyFrames.Clone());

            OnFreezablePropertyChanged(null, _keyFrames);
        }

        private static TKeyFrameCollection GetEmptyKeyFrames() {
            if(_emptyKeyFrames == null) {
                var empty = new TKeyFrameCollection();
                empty.Freeze();
                _emptyKeyFrames = empty;
            }
            return _emptyKeyFrames;
        }

        private IList<KeyFrame<TValue>> KeyFrameList {
            get { return _keyFrames; }
        }
        #endregion

        #region Inner types
        private struct KeyFrameRef : IComparable<KeyFrameRef> {
            public int OriginalIndex;
            public TimeSpan ResolvedTime;

            #region IComparable
            public int CompareTo(KeyFrameRef other) {
                var res = ResolvedTime.CompareTo(other.ResolvedTime);
                return res != 0 ? res : OriginalIndex - other.OriginalIndex;
            }
            #endregion
        }

        private struct KeyFrameRange {
            public int FirstIndex;
            public int LastIndex;
        }
        #endregion

        #region Fields
        private TKeyFrameCollection _keyFrames;
        private static TKeyFrameCollection _emptyKeyFrames;
        private KeyFrameRef[] _keyFrameRefs;
        private bool _keyFramesResolved;

        protected static readonly IAnimationHelper<TValue> AnimationHelper = SingletonOf<TAnimationHelper>.Instance;
        #endregion
    }
}
