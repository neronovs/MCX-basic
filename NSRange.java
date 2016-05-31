package com.neronov.aleksei.mcxbasic;

/**
 * Created by newuser on 14.03.16.
 */
public class NSRange {
    public int location;
    public int length;

    NSRange(int location, int length) {
        this.location = location;
        this.length = length;
    }

    public int getLocation() {
        return this.location;
    }

    public void setLocaion(int value) {
        location = value;
    }

    public int getLength() {
        return this.length;
    }

    public void setLength(int value) {
        length = value;
    }

}