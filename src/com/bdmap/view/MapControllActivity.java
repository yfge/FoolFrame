package com.bdmap.view;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

import com.baidu.location.BDLocation;
import com.baidu.location.BDLocationListener;
import com.baidu.location.LocationClient;
import com.baidu.location.LocationClientOption;
import com.baidu.mapapi.map.BaiduMap;
import com.baidu.mapapi.map.BaiduMap.OnMapClickListener;
import com.baidu.mapapi.map.BaiduMap.SnapshotReadyCallback;
import com.baidu.mapapi.map.BitmapDescriptor;
import com.baidu.mapapi.map.MapPoi;
import com.baidu.mapapi.map.MapStatus;
import com.baidu.mapapi.map.MapStatusUpdate;
import com.baidu.mapapi.map.MapStatusUpdateFactory;
import com.baidu.mapapi.map.MapView;
import com.baidu.mapapi.map.MyLocationConfiguration;
import com.baidu.mapapi.map.MyLocationConfiguration.LocationMode;
import com.baidu.mapapi.map.MyLocationData;
import com.baidu.mapapi.model.LatLng;

import android.app.Activity;
import android.graphics.Bitmap;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.Toast;

/**
 * 地图控制demo（点击、双击、长按、缩放、旋转、俯视） + 定位
 * 
 * @author ys
 *
 */
public class MapControllActivity extends Activity implements OnClickListener {
	// 地图控件对象
	private MapView mapView;
	// 百度地图对象
	private BaiduMap bdMap;
	// 经纬度
	private double latitude, longitude;
	// 缩小
	private Button zoomOutBtn;
	// 放大
	private Button zoomInBtn;
	// 旋转
	private Button rotateBtn;
	// 俯视
	private Button overlookBtn;
	// 截图
	private Button screenShotBtn;

	// 标记是否已经放大到最大或者缩小到最小级别
	private boolean isMaxOrMin = false;

	private float maxZoom = 0.0f;
	private float minZoom = 0.0f;
	// 记录当前地图的缩放级别
	private float currentZoom = 0.0f;
	// 描述地图状态将要发生的状态
	private MapStatusUpdate msu;
	// 用于生成地图将要发生的变化
	private MapStatusUpdateFactory msuFactory;
	// 定义地图状态
	private MapStatus mapStatus;

	// 旋转角度
	private float rotateAngle = 0.0f;
	// 俯视角度 （0 ~ -45°）
	private float overlookAngle = 0.0f;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_map_controll);
		init();
	}

	/**
	 * 
	 */
	private void init() {
		mapView = (MapView) findViewById(R.id.bd_mapview);
		bdMap = mapView.getMap();

		mapView.showZoomControls(false);// 不显示默认的缩放控件
		mapView.showScaleControl(false);// 不显示默认比例尺控件

		maxZoom = bdMap.getMaxZoomLevel();// 获得地图的最大缩放级别
		minZoom = bdMap.getMinZoomLevel();// 获得地图的最小缩放级别

		zoomInBtn = (Button) findViewById(R.id.zoom_in_btn);
		zoomOutBtn = (Button) findViewById(R.id.zoom_out_btn);
		rotateBtn = (Button) findViewById(R.id.rotate_btn);
		overlookBtn = (Button) findViewById(R.id.overlook_btn);
		screenShotBtn = (Button) findViewById(R.id.screen_shot_btn);

		zoomInBtn.setOnClickListener(this);
		zoomOutBtn.setOnClickListener(this);
		rotateBtn.setOnClickListener(this);
		overlookBtn.setOnClickListener(this);
		screenShotBtn.setOnClickListener(this);

		bdMap.setOnMapClickListener(new OnMapClickListener() {
			@Override
			public boolean onMapPoiClick(MapPoi arg0) {
				return false;
			}

			@Override
			public void onMapClick(LatLng arg0) {
				// 设置地图新中心点
				msu = msuFactory.newLatLng(arg0);
				bdMap.animateMapStatus(msu);
				Toast.makeText(MapControllActivity.this,
						"地图中心点移动到：" + arg0.toString(), Toast.LENGTH_SHORT)
						.show();
			}
		});

	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.zoom_out_btn:// 缩小
			msu = msuFactory.zoomOut();
			bdMap.animateMapStatus(msu);
			currentZoom = bdMap.getMapStatus().zoom;
			Toast.makeText(MapControllActivity.this,
					"当前地图的缩放级别是：" + currentZoom, Toast.LENGTH_SHORT).show();
			break;
		case R.id.zoom_in_btn:// 放大
			msu = msuFactory.zoomIn();
			bdMap.animateMapStatus(msu);
			currentZoom = bdMap.getMapStatus().zoom;
			Toast.makeText(MapControllActivity.this,
					"当前地图的缩放级别是：" + currentZoom, Toast.LENGTH_SHORT).show();
			break;
		case R.id.rotate_btn:// 旋转
			mapStatus = new MapStatus.Builder(bdMap.getMapStatus()).rotate(
					rotateAngle += 30).build();
			msu = msuFactory.newMapStatus(mapStatus);
			bdMap.animateMapStatus(msu);
			break;
		case R.id.overlook_btn:// 俯视
			mapStatus = new MapStatus.Builder(bdMap.getMapStatus()).overlook(
					overlookAngle -= 10).build();
			msu = msuFactory.newMapStatus(mapStatus);
			bdMap.animateMapStatus(msu);
			break;
		case R.id.screen_shot_btn:// 截图
			bdMap.snapshot(new SnapshotReadyCallback() {
				@Override
				public void onSnapshotReady(Bitmap bitmap) {
					File file = new File("/mnt/sdcard/test.png");
					FileOutputStream out;
					try {
						out = new FileOutputStream(file);
						if (bitmap
								.compress(Bitmap.CompressFormat.PNG, 100, out)) {
							out.flush();
							out.close();
						}
						Toast.makeText(MapControllActivity.this,
								"屏幕截图成功，图片存在: " + file.toString(),
								Toast.LENGTH_SHORT).show();
					} catch (FileNotFoundException e) {
						e.printStackTrace();
					} catch (IOException e) {
						e.printStackTrace();
					}
				}
			});
			break;
		default:
			break;
		}
	}

	@Override
	protected void onPause() {
		super.onPause();
		mapView.onPause();
	}

	@Override
	protected void onResume() {
		super.onResume();
		mapView.onResume();
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();
		mapView.onDestroy();
	}

}
