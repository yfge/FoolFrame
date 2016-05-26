package com.bdmap.view;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.Button;
import com.baidu.mapapi.map.BaiduMap;
import com.baidu.mapapi.map.MapStatusUpdate;
import com.baidu.mapapi.map.MapStatusUpdateFactory;
import com.baidu.mapapi.map.MapView;

public class BasisMapActivity extends Activity implements OnClickListener {
	// 百度地图控件
	private MapView mMapView = null;
	// 百度地图对象
	private BaiduMap bdMap;
	// 普通地图
	private Button normalMapBtn;
	// 卫星地图
	private Button satelliteMapBtn;
	// 实时路况交通图
	private Button trafficMapBtn;
	// 热力图
	private Button headMapBtn;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		setContentView(R.layout.activity_basis_map);
		init();
	}

	/**
	 * 初始化方法
	 */
	private void init() {
		mMapView = (MapView) findViewById(R.id.bmapview);
		mMapView.showZoomControls(false);// 不显示默认的缩放控件

		MapStatusUpdate msu = MapStatusUpdateFactory.zoomTo(15.0f);
		bdMap = mMapView.getMap();
		bdMap.setMapStatus(msu);

		normalMapBtn = (Button) findViewById(R.id.normal_map_btn);
		satelliteMapBtn = (Button) findViewById(R.id.satellite_map_btn);
		trafficMapBtn = (Button) findViewById(R.id.traffic_map_btn);
		headMapBtn = (Button) findViewById(R.id.heat_map_btn);
		
		normalMapBtn.setOnClickListener(this);
		satelliteMapBtn.setOnClickListener(this);
		trafficMapBtn.setOnClickListener(this);
		headMapBtn.setOnClickListener(this);

		//
		normalMapBtn.setEnabled(false);
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.normal_map_btn:
			bdMap.setMapType(BaiduMap.MAP_TYPE_NORMAL);
			normalMapBtn.setEnabled(false);
			satelliteMapBtn.setEnabled(true);
			break;
		case R.id.satellite_map_btn:
			bdMap.setMapType(BaiduMap.MAP_TYPE_SATELLITE);
			satelliteMapBtn.setEnabled(false);
			normalMapBtn.setEnabled(true);
			break;
		case R.id.traffic_map_btn:
			if (!bdMap.isTrafficEnabled()) {
				bdMap.setTrafficEnabled(true);
				trafficMapBtn.setText("关闭实时路况");
			} else {
				bdMap.setTrafficEnabled(false);
				trafficMapBtn.setText("打开实时路况");
			}
			break;
		case R.id.heat_map_btn:
			if (!bdMap.isBaiduHeatMapEnabled()) {
				bdMap.setBaiduHeatMapEnabled(true);
				headMapBtn.setText("关闭热力图");
			} else {
				bdMap.setBaiduHeatMapEnabled(false);
				headMapBtn.setText("打开热力图");
			}
			break;
		}
	}

	@Override
	protected void onResume() {
		super.onResume();
		mMapView.onResume();
	}

	@Override
	protected void onPause() {
		super.onPause();
		mMapView.onPause();
	}

	@Override
	protected void onDestroy() {
		mMapView.onDestroy();
		mMapView = null;
		super.onDestroy();
	}

}
