package com.bdmap.view;

import android.content.Context;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;

public class MyOrientationListener implements SensorEventListener{

	private SensorManager sensorManager;
	private Context context;
	private Sensor sensor;
	
	private float lastX;
	
	public MyOrientationListener(Context context) {
		this.context =context;
	}
	
	public void start(){
		sensorManager = (SensorManager) context.getSystemService(Context.SENSOR_SERVICE);
		if (sensorManager != null) {
			//获得方向传感器
			sensor = sensorManager.getDefaultSensor(Sensor.TYPE_ORIENTATION);
		}
		if (sensor != null) {
			sensorManager.registerListener(this, sensor, sensorManager.SENSOR_DELAY_UI);
		}
	}
	
	public void stop(){
		sensorManager.unregisterListener(this);
	}
	
	
	@Override
	public void onSensorChanged(SensorEvent event) {
		if (event.sensor.getType() == Sensor.TYPE_ORIENTATION) {
			float x = event.values[SensorManager.DATA_X];
			if (Math.abs(x - lastX) > 1.0) {
				if (onOrietationListener != null) {
					onOrietationListener.onOrientationChanged(x);
				}
			}
			lastX = x;
		}
	}

	@Override
	public void onAccuracyChanged(Sensor sensor, int accuracy) {
		
	}
	
	private OnOrietationListener onOrietationListener;
	
	
	public OnOrietationListener getOnOrietationListener() {
		return onOrietationListener;
	}

	public void setOnOrietationListener(OnOrietationListener onOrietationListener) {
		this.onOrietationListener = onOrietationListener;
	}


	public interface OnOrietationListener{
		void onOrientationChanged(float x);
	}

}
