# Splines Editor (Unity 6, C#)

## Author

> **Morgane DERO** - Master 1 Game Programmer at Isart Digital 

## Description

Tool made in Unity 6 to create, edit and visualize spline curves (Hermite, Bézier, B‑Spline, Catmull‑Rom) and animate GameObjects along them.

## Project Overview

### This project provides :
* Mathematical implementation of 4 spline types in C#.
* An in‑editor tool to create and edit splines via control points.
* Runtime animation of one or several GameObjects along a selected spline.

### Main scripts:
* Hermite.cs – Hermite curve evaluation + tangent computation.
* Bezier.cs – Cubic Bézier curve evaluation + test scenarios.
* B_Spline.cs – Uniform cubic B‑Spline evaluation.
* Catmull_Rom.cs – Catmull‑Rom spline evaluation.
* SplineManager.cs – Manages all splines in the scene and their control points.
* SplineAnimation.cs – Instantiates followers and moves them along the current spline.
* AlgorithmSelection.cs – Chooses the curve type per spline and runs tests.
* ControlPoint.cs – Component attached to each control point (tension for Hermite).
* DrawInEditor.cs (Editor only) – Draws control polygons and spline curves in the Scene view.
* AlgoManagerEditor.cs (Editor only) – Custom inspectors and buttons to create splines / points.

## Mathematical implementation 

Each curve type is implemented using its standard matrix / polynomial form:   
* Hermite
  *   Hermite.SH(P0, P1, T0, T1, t) computes a point from 2 positions and 2 tangents using Hermite basis functions.
  *   Hermite.Tangent(controlPoints, i, tension) computes per‑point tangents from control points and a tension parameter.
* Bezier
  * Bezier.SB(P0, P1, P2, P3, t) evaluates a cubic Bézier curve between 4 control points.
  * Bezier.Testing() logs several test scenarios (aligned points, semicircle, S‑curve, etc.) to validate the implementation.
* B-Spline
  * B_Spline.SBS(P0, P1, P2, P3, t) evaluates a uniform cubic B‑Spline segment from 4 consecutive control points using the classic B‑Spline basis divided by 6.
* Catmull‑Rom
  * Catmull_Rom.SCR(P0, P1, P2, P3, t) evaluates a Catmull‑Rom segment from 4 control points, interpolating P1 and P2 with C1 continuity.     


All functions work in 3D (Vector3) and assume t ∈ [0,1].

## Editor usage
1. Open the scene containing a SplineManager GameObject.
2. In the SplineManager inspector, click “New Spline”:
   * A new Spline_X GameObject is created.
   * It receives AlgorithmSelection and SplineAnimation.
   * Two initial ControlPoint children (cubes) are created.
3. On Spline_X:
   * In AlgorithmSelection, choose one of: Hermite, Bézier, B‑Spline, Catmull‑Rom.
   * Use the “Add new control point” button (custom inspector) to insert more control points.
   * Move control points in the Scene view to shape the curve.
4. Visualization:
   * DrawInEditor draws:
     * Black polyline between control points.
     * Colored spline curve:
       * Red: Hermite
       * Magenta: Bézier
       * Green: B‑Spline
       * Cyan: Catmull‑Rom

## Animation usage
On each Spline_X:
1. In SplineAnimation:
   * Assign one or several prefabs to m_ObjectToAnimate.
   * Set speed (curve progress speed).
   * Optionally enable loop to restart at t = 0 when t ≥ 1.
2. At runtime:
   * On Awake, SplineAnimation caches control points and instantiates followers at the first control point.
   * In Update, a shared parameter pos_t ∈ [0,1] advances over the spline.
   * For each follower i, an offset offsetT = Clamp01(pos_t + i * 0.1) is used to spread objects along the curve.
   * EvaluateSpline selects the correct evaluation function (Hermite / Bézier / B‑Spline / Catmull‑Rom) and converts t into segment index + local parameter for multi‑segment splines.
     
Result: all assigned objects move smoothly along the selected spline type, each slightly offset in time.

## Project structure and build

*  Requires Unity 6 and C#.
* Editor scripts (DrawInEditor, AlgoManagerEditor) are wrapped in #if UNITY_EDITOR and placed in EditorScripts so they are excluded from builds.
* SplineManager is marked [ExecuteAlways] to keep the spline list updated in Edit and Play modes.
* The project is intended to compile without any errors or warnings in both Editor and Player builds, as required in the assignment.











