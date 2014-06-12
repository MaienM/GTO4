﻿using UnityEngine;

public class HoverButton
{
    static int HoverButtonHash = "HoverButton".GetHashCode();

    private Rect startRect = default(Rect);
    private Vector3 offset;
    private GUIContent content;
    private GUIStyle style;
    private Object value;

    private Rect currentRect
    {
        get
        {
            return new Rect(startRect.x + offset.x, startRect.y + offset.y, startRect.width, startRect.height);
        }
    }

    public HoverButton(Rect position, GUIContent content, GUIStyle style, Object value)
    {
        this.startRect = position;
        this.content = content;
        this.style = style;
        this.value = value;
    }

    public void OnGUI()
    {
        switch (DrawHoverButton(currentRect, content, style))
        {
            case EventType.mouseDown:
                break;

            case EventType.mouseUp:
                GameObject.FindGameObjectWithTag("GameController").SendMessage("onUnitDrop", value);
                offset = Vector3.zero;
                break;

            case EventType.mouseDrag:
                offset.x = Input.mousePosition.x - startRect.x - (startRect.width / 2);
                offset.y = (Screen.height - Input.mousePosition.y) - startRect.y - (startRect.height / 2);
                break;
        }
    }

    private static EventType DrawHoverButton(Rect position, GUIContent content, GUIStyle style)
    {
        int controlID = GUIUtility.GetControlID(HoverButtonHash, FocusType.Native);
        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.mouseDown:
                if (position.Contains(Event.current.mousePosition))
                {
                    GUIUtility.hotControl = controlID;
                    Event.current.Use();
                    return EventType.mouseDown;
                }
                break;
            case EventType.mouseUp:
                if (GUIUtility.hotControl != controlID)
                    return EventType.ignore;
                GUIUtility.hotControl = 0;
                Event.current.Use();
                if (position.Contains(Event.current.mousePosition))
                    return EventType.mouseUp;
                else
                    return EventType.ignore;
            case EventType.mouseDrag:
                if (GUIUtility.hotControl == controlID)
                {
                    Event.current.Use();
                    return EventType.mouseDrag;
                }
                else
                    return EventType.ignore;
            case EventType.repaint:
                style.Draw(position, content, controlID);
                if (position.Contains(Event.current.mousePosition))
                    return EventType.mouseMove;
                else
                    return EventType.repaint;
        }
        if (position.Contains(Event.current.mousePosition))
            return EventType.mouseMove;
        else
            return EventType.ignore;
    }
}