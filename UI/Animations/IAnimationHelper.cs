// ==================================================
// Copyright 2016(C) , DotLogix
// File:  IAnimationHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

namespace DotLogix.UI.Animations {
    /// <summary>
    ///     Helper actions for animation of the TValue type properties
    /// </summary>
    /// <typeparam name="TValue"> </typeparam>
    public interface IAnimationHelper<TValue> {
        /// <summary>
        ///     If false than properties IsAdditive and IsCumulative will be ignored for this type animation
        /// </summary>
        bool IsAccumulable { get; }

        /// <summary>
        ///     Verifies the value for validity
        /// </summary>
        /// <param name="value"> Value to verify </param>
        /// <returns> true if value is valid </returns>
        bool IsValidValue(TValue value);

        /// <summary>
        ///     Returns valid value, adding which to or subtracting from other value leaves this other value unchanged
        /// </summary>
        /// <returns> valid value </returns>
        TValue GetZeroValue();

        /// <summary>
        ///     Returns sum of two values
        /// </summary>
        /// <param name="value1"> first valid value </param>
        /// <param name="value2"> second valid value </param>
        /// <returns> sum </returns>
        TValue AddValues(TValue value1, TValue value2);

        /// <summary>
        ///     Subtracts values
        /// </summary>
        /// <param name="value1"> first valid value </param>
        /// <param name="value2"> second valid value </param>
        TValue SubtractValue(TValue value1, TValue value2);

        /// <summary>
        ///     Multiplies value by factor
        /// </summary>
        /// <param name="value"> value </param>
        /// <param name="factor"> factor </param>
        /// <returns> scaled value </returns>
        TValue ScaleValue(TValue value, double factor);

        /// <summary>
        ///     Returns interpolated value from the specified range according the progress
        /// </summary>
        /// <param name="from"> beginning of the range </param>
        /// <param name="to"> end of the range </param>
        /// <param name="progress"> progress in the range. Valid values are in range from 0.0 to 1.0 </param>
        /// <returns> interpolated value </returns>
        TValue InterpolateValue(TValue from, TValue to, double progress);

        /// <summary>
        ///     Calculates the relative delta between values of two key frames. If segment length cannot be calculated return 1.0
        ///     if values are different and 0.0 if the same.
        /// </summary>
        /// <param name="from"> value of the previous key frame </param>
        /// <param name="to"> value of the current key frame </param>
        /// <returns> calculated segment length </returns>
        double GetSegmentLength(TValue from, TValue to);
    }
}
