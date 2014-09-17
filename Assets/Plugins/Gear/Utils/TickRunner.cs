//
// EnterFrameTimer.cs
//
// Author:
//       Hongbin.Yang <>
//
// Copyright (c) 2014 Hongbin.Yang
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class TickRunner{
	private ArrayList tickerList;
	private uint prevMSTimeStamp;
	private static TickRunner instance;

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <returns>The instance.</returns>
	public static TickRunner GetInstance(){
		if (instance == null) {
			instance = new TickRunner();

		}
		return instance;
	}

	/// <summary>
	/// Init this instance.
	/// </summary>
	public void Init(){
		tickerList = new ArrayList();
		prevMSTimeStamp = (uint)(Time.time * 1000);
	}

	/// <summary>
	/// Adds the ticker.
	/// </summary>
	/// <param name="ticker">Ticker.</param>
	/// <param name="delayMSTime">Delay MS time.</param>
	public void AddTicker(ITicker ticker, int delayMSTime = -1){
		uint lastMSTimeStamp = (uint)(Time.time * 1000);
		TickVO tempTicker;
		if (HasTicker(ticker)) {
			//如果已经存在，那么更新下他
			tempTicker = GetTicker(ticker);
		} else {
			//没有则新建一个
			tempTicker = new TickVO();
		}
		tempTicker.ticker = ticker;
		tempTicker.delayMSTime = delayMSTime;
		tempTicker.lastMSTimeStamp = lastMSTimeStamp;
		tickerList.Add(tempTicker);
	}

	/// <summary>
	/// Determines whether this instance has ticker the specified ticker.
	/// </summary>
	/// <returns><c>true</c> if this instance has ticker the specified ticker; otherwise, <c>false</c>.</returns>
	/// <param name="ticker">Ticker.</param>
	public bool HasTicker(ITicker ticker){
            
		foreach (TickVO t in tickerList) {
			if (t.ticker == ticker) {
				return true;
			}
		}
		return false;
	}

	private TickVO GetTicker(ITicker ticker){
        if (tickerList == null) {
            return null;
        }
		foreach (TickVO t in tickerList) {
			if (t.ticker == ticker) {
				return t;
			}
		}
		return null;
	}

	/// <summary>
	/// Removes the ticker.
	/// </summary>
	/// <param name="ticker">Ticker.</param>
	public void RemoveTicker(ITicker ticker){
		foreach (TickVO t in tickerList) {
			if (t.ticker == ticker) {
				t.ticker = null;
			}
		}
	}

	// Update is called once per frame
	/// <summary>
	/// Update this instance.
	/// </summary>
	public void Update(){
		uint currentMSTimeStamp = (uint)(Time.time * 1000);
		uint delay = (currentMSTimeStamp - prevMSTimeStamp);
            
		foreach (TickVO t in tickerList) {
			if (t.ticker == null) {
				continue;
			}
			if (t.delayMSTime == -1 || currentMSTimeStamp - t.lastMSTimeStamp >= t.delayMSTime) {
				//当符合大于延迟时间，那么调用下
				t.ticker.OnTick();
				t.lastMSTimeStamp = currentMSTimeStamp;
			}
		}
		prevMSTimeStamp = currentMSTimeStamp;

		//清理下要删除的TickVO
		foreach (TickVO removeTick in tickerList) {
			if (removeTick.ticker == null) {
				tickerList.Remove(removeTick);
				break;
			}
		}
	}

	class TickVO{
		public ITicker ticker;
		public int delayMSTime = -1;
		public uint lastMSTimeStamp;
	}

}
