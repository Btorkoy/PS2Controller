import serial
import time
import os
from pynput.keyboard import Key, Controller

keyboard = Controller()

delta_time = 0.05

ser = serial.Serial('COM3', 57600, timeout=0)

keys = [
    ['0',     False,  0.0],     #select 
    ['1',     False,  0.0],     #start 
    ['2',     False,  0.0],     #squire 
    ['3',     False,  0.0],     #cross 
    ['4',     False,  0.0],     #down 
    ['5',     False,  0.0],     #left 
    ['6',     False,  0.0],     #right
    ['7',     False,  0.0],     #up
    #['8',     False,  0.0],     #start
    #['9',     False,  0.0],     #start
    # 1 - left V 
    # 2 - left H
    # 3 - right V
    # 4 - right H775
]

while 1:
    data  = ser.read()
    if data:   
        keys_state = ord(data)
        for i in range(8):	
            key = keys[i]
            if bool(keys_state & (1 << i)) == key[1] and time.time() - key[2] > delta_time:
                print(key)
                if not key[1]:
                    keyboard.release(key[0])
                else:
                    keyboard.press(key[0])
                key[1] = not key[1]
                key[2] = time.time()
        
